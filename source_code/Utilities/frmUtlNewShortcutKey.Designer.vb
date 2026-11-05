<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlNewShortcutKey
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cboKey = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkShift = New System.Windows.Forms.CheckBox()
        Me.chkAlt = New System.Windows.Forms.CheckBox()
        Me.chkCtrl = New System.Windows.Forms.CheckBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnOk = New C1.Win.C1Input.C1Button()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cboKey)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.chkShift)
        Me.Panel1.Controls.Add(Me.chkAlt)
        Me.Panel1.Controls.Add(Me.chkCtrl)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(304, 94)
        Me.Panel1.TabIndex = 0
        '
        'cboKey
        '
        Me.cboKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboKey.FormattingEnabled = True
        Me.cboKey.Items.AddRange(New Object() {"Backspace", "Tab", "Enter", "Escape", "Spacebar", "Page up", "Page down", "End", "Home", "Left arrow", "Up arrow", "Right arrow", "Down arrow", "Insert", "Delete", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"})
        Me.cboKey.Location = New System.Drawing.Point(170, 24)
        Me.cboKey.Name = "cboKey"
        Me.cboKey.Size = New System.Drawing.Size(121, 21)
        Me.cboKey.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(139, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(25, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Key"
        '
        'chkShift
        '
        Me.chkShift.AutoSize = True
        Me.chkShift.Location = New System.Drawing.Point(62, 58)
        Me.chkShift.Name = "chkShift"
        Me.chkShift.Size = New System.Drawing.Size(47, 17)
        Me.chkShift.TabIndex = 4
        Me.chkShift.Text = "Shift"
        Me.chkShift.UseVisualStyleBackColor = True
        '
        'chkAlt
        '
        Me.chkAlt.AutoSize = True
        Me.chkAlt.Location = New System.Drawing.Point(62, 35)
        Me.chkAlt.Name = "chkAlt"
        Me.chkAlt.Size = New System.Drawing.Size(38, 17)
        Me.chkAlt.TabIndex = 3
        Me.chkAlt.Text = "Alt"
        Me.chkAlt.UseVisualStyleBackColor = True
        '
        'chkCtrl
        '
        Me.chkCtrl.AutoSize = True
        Me.chkCtrl.Location = New System.Drawing.Point(62, 14)
        Me.chkCtrl.Name = "chkCtrl"
        Me.chkCtrl.Size = New System.Drawing.Size(41, 17)
        Me.chkCtrl.TabIndex = 2
        Me.chkCtrl.Text = "Ctrl"
        Me.chkCtrl.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.vLQMS.My.Resources.Resources.hotkey1
        Me.PictureBox1.Location = New System.Drawing.Point(12, 13)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(44, 41)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(182, 113)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(87, 33)
        Me.btnCancel.TabIndex = 18
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOk.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOk.Location = New System.Drawing.Point(74, 113)
        Me.btnOk.Margin = New System.Windows.Forms.Padding(4)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 33)
        Me.btnOk.TabIndex = 17
        Me.btnOk.Text = "&Ok"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'frmUtlNewShortcutKey
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(328, 159)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtlNewShortcutKey"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New shortcut key..."
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents cboKey As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkShift As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlt As System.Windows.Forms.CheckBox
    Friend WithEvents chkCtrl As System.Windows.Forms.CheckBox
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnOk As C1.Win.C1Input.C1Button
End Class
