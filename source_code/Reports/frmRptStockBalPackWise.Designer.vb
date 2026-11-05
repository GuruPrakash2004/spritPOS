<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptStockBalPackWise
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
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.chkSelectedGroup = New System.Windows.Forms.CheckBox()
        Me.grbForThePeriod = New System.Windows.Forms.GroupBox()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.grbForThePeriod.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(434, 110)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(70, 30)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "&Close"
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut:Alt+C")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(347, 110)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 30)
        Me.btnPrint.TabIndex = 6
        Me.btnPrint.Text = "&Print"
        Me.ToolTip1.SetToolTip(Me.btnPrint, "Shortcut: Alt+P")
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'chkSelectedGroup
        '
        Me.chkSelectedGroup.AutoSize = True
        Me.chkSelectedGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelectedGroup.Location = New System.Drawing.Point(17, 80)
        Me.chkSelectedGroup.Name = "chkSelectedGroup"
        Me.chkSelectedGroup.Size = New System.Drawing.Size(140, 24)
        Me.chkSelectedGroup.TabIndex = 5
        Me.chkSelectedGroup.Text = "Selected Group"
        Me.chkSelectedGroup.UseVisualStyleBackColor = True
        '
        'grbForThePeriod
        '
        Me.grbForThePeriod.Controls.Add(Me.dtpToDate)
        Me.grbForThePeriod.Controls.Add(Me.lblTo)
        Me.grbForThePeriod.Controls.Add(Me.dtpFromDate)
        Me.grbForThePeriod.Controls.Add(Me.lblFrom)
        Me.grbForThePeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbForThePeriod.Location = New System.Drawing.Point(12, 13)
        Me.grbForThePeriod.Name = "grbForThePeriod"
        Me.grbForThePeriod.Size = New System.Drawing.Size(330, 59)
        Me.grbForThePeriod.TabIndex = 4
        Me.grbForThePeriod.TabStop = False
        Me.grbForThePeriod.Text = "For The Period "
        '
        'dtpToDate
        '
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpToDate.Location = New System.Drawing.Point(205, 22)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(106, 26)
        Me.dtpToDate.TabIndex = 3
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(172, 26)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(27, 20)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        '
        'dtpFromDate
        '
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFromDate.Location = New System.Drawing.Point(61, 22)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(106, 26)
        Me.dtpFromDate.TabIndex = 1
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(11, 26)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(46, 20)
        Me.lblFrom.TabIndex = 0
        Me.lblFrom.Text = "From"
        '
        'frmRptStockBalPackWise
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(522, 151)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.chkSelectedGroup)
        Me.Controls.Add(Me.grbForThePeriod)
        Me.Name = "frmRptStockBalPackWise"
        Me.ShowIcon = False
        Me.Text = "Closing Stock Report-Package Wise"
        Me.grbForThePeriod.ResumeLayout(False)
        Me.grbForThePeriod.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnClose As Button
    Friend WithEvents btnPrint As Button
    Friend WithEvents chkSelectedGroup As CheckBox
    Friend WithEvents grbForThePeriod As GroupBox
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents lblTo As Label
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents lblFrom As Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
