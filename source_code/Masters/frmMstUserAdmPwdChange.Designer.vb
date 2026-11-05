<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMstUserAdmPwdChange
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstUserAdmPwdChange))
        Me.lblAdmPwd = New System.Windows.Forms.Label()
        Me.txtAdmPwd = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtOldAdmPwd = New System.Windows.Forms.TextBox()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.imgEye = New System.Windows.Forms.PictureBox()
        CType(Me.imgEye, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblAdmPwd
        '
        Me.lblAdmPwd.AutoSize = True
        Me.lblAdmPwd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdmPwd.ForeColor = System.Drawing.Color.Red
        Me.lblAdmPwd.Location = New System.Drawing.Point(12, 9)
        Me.lblAdmPwd.Name = "lblAdmPwd"
        Me.lblAdmPwd.Size = New System.Drawing.Size(170, 20)
        Me.lblAdmPwd.TabIndex = 517
        Me.lblAdmPwd.Tag = "M"
        Me.lblAdmPwd.Text = "Adnl ADMIN Password"
        '
        'txtAdmPwd
        '
        Me.txtAdmPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAdmPwd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdmPwd.Location = New System.Drawing.Point(114, 75)
        Me.txtAdmPwd.MaxLength = 50
        Me.txtAdmPwd.Name = "txtAdmPwd"
        Me.txtAdmPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtAdmPwd.Size = New System.Drawing.Size(219, 26)
        Me.txtAdmPwd.TabIndex = 516
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(68, 77)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 20)
        Me.Label7.TabIndex = 520
        Me.Label7.Tag = "M"
        Me.Label7.Text = "New"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(75, 47)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 20)
        Me.Label4.TabIndex = 519
        Me.Label4.Tag = "M"
        Me.Label4.Text = "Old"
        '
        'txtOldAdmPwd
        '
        Me.txtOldAdmPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtOldAdmPwd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOldAdmPwd.Location = New System.Drawing.Point(114, 43)
        Me.txtOldAdmPwd.MaxLength = 50
        Me.txtOldAdmPwd.Name = "txtOldAdmPwd"
        Me.txtOldAdmPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtOldAdmPwd.Size = New System.Drawing.Size(219, 26)
        Me.txtOldAdmPwd.TabIndex = 518
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(177, 120)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdate.TabIndex = 521
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(258, 120)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 522
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'imgEye
        '
        Me.imgEye.Image = CType(resources.GetObject("imgEye.Image"), System.Drawing.Image)
        Me.imgEye.Location = New System.Drawing.Point(339, 43)
        Me.imgEye.Name = "imgEye"
        Me.imgEye.Size = New System.Drawing.Size(30, 24)
        Me.imgEye.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgEye.TabIndex = 523
        Me.imgEye.TabStop = False
        '
        'frmMstUserAdmPwdChange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(377, 167)
        Me.Controls.Add(Me.imgEye)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.txtAdmPwd)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.lblAdmPwd)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtOldAdmPwd)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMstUserAdmPwdChange"
        Me.Text = "Change Additional ADMIN password"
        CType(Me.imgEye, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtAdmPwd As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents lblAdmPwd As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtOldAdmPwd As TextBox
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents imgEye As PictureBox
End Class
