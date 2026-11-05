<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlDaysExpOPBalance
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlDaysExpOPBalance))
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.dtDocDate = New System.Windows.Forms.DateTimePicker()
        Me.lblDocDate = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtOPAmt = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(338, 142)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 30)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(256, 142)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 30)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'dtDocDate
        '
        Me.dtDocDate.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.dtDocDate.CustomFormat = "dd/MM/yyyy"
        Me.dtDocDate.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDocDate.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.dtDocDate.Location = New System.Drawing.Point(184, 26)
        Me.dtDocDate.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.dtDocDate.Name = "dtDocDate"
        Me.dtDocDate.Size = New System.Drawing.Size(110, 27)
        Me.dtDocDate.TabIndex = 1
        '
        'lblDocDate
        '
        Me.lblDocDate.AutoSize = True
        Me.lblDocDate.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocDate.ForeColor = System.Drawing.Color.Red
        Me.lblDocDate.Location = New System.Drawing.Point(120, 31)
        Me.lblDocDate.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblDocDate.Name = "lblDocDate"
        Me.lblDocDate.Size = New System.Drawing.Size(42, 20)
        Me.lblDocDate.TabIndex = 0
        Me.lblDocDate.Tag = "R"
        Me.lblDocDate.Text = "Date"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(80, 73)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(300, 32)
        Me.Label1.TabIndex = 2
        Me.Label1.Tag = "R"
        Me.Label1.Text = "Cash opening balance as on this date"
        '
        'txtOPAmt
        '
        Me.txtOPAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOPAmt.Location = New System.Drawing.Point(38, 99)
        Me.txtOPAmt.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.txtOPAmt.Name = "txtOPAmt"
        Me.txtOPAmt.Size = New System.Drawing.Size(368, 30)
        Me.txtOPAmt.TabIndex = 3
        Me.txtOPAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'frmUtlDaysExpOPBalance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(430, 184)
        Me.Controls.Add(Me.txtOPAmt)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dtDocDate)
        Me.Controls.Add(Me.lblDocDate)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtlDaysExpOPBalance"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opening Balance (Petty Cash / Expense)"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Public WithEvents dtDocDate As DateTimePicker
    Friend WithEvents lblDocDate As Label
    Friend WithEvents Label1 As Label
    Public WithEvents txtOPAmt As TextBox
End Class
