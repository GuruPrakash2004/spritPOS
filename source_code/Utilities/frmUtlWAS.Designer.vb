<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlWAS
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
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnGetAPI = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtMSGAPI = New System.Windows.Forms.TextBox()
        Me.txtMsgFileAPI = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDMOBNO = New System.Windows.Forms.TextBox()
        Me.txtEmoji = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(570, 263)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(82, 29)
        Me.btnClose.TabIndex = 46
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnGetAPI
        '
        Me.btnGetAPI.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGetAPI.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnGetAPI.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGetAPI.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnGetAPI.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGetAPI.Location = New System.Drawing.Point(15, 263)
        Me.btnGetAPI.Name = "btnGetAPI"
        Me.btnGetAPI.Size = New System.Drawing.Size(80, 29)
        Me.btnGetAPI.TabIndex = 288
        Me.btnGetAPI.Text = "Get API"
        Me.btnGetAPI.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(482, 263)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(82, 29)
        Me.btnSave.TabIndex = 289
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 16)
        Me.Label1.TabIndex = 290
        Me.Label1.Text = "Message API"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 122)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(143, 16)
        Me.Label2.TabIndex = 291
        Me.Label2.Text = "Message With File API"
        '
        'txtMSGAPI
        '
        Me.txtMSGAPI.Location = New System.Drawing.Point(179, 13)
        Me.txtMSGAPI.MaxLength = 1000
        Me.txtMSGAPI.Multiline = True
        Me.txtMSGAPI.Name = "txtMSGAPI"
        Me.txtMSGAPI.Size = New System.Drawing.Size(473, 63)
        Me.txtMSGAPI.TabIndex = 292
        '
        'txtMsgFileAPI
        '
        Me.txtMsgFileAPI.Location = New System.Drawing.Point(179, 91)
        Me.txtMsgFileAPI.MaxLength = 1000
        Me.txtMsgFileAPI.Multiline = True
        Me.txtMsgFileAPI.Name = "txtMsgFileAPI"
        Me.txtMsgFileAPI.Size = New System.Drawing.Size(473, 71)
        Me.txtMsgFileAPI.TabIndex = 293
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(11, 187)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(161, 16)
        Me.Label3.TabIndex = 294
        Me.Label3.Text = "Default Receiver MOBNO"
        '
        'txtDMOBNO
        '
        Me.txtDMOBNO.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDMOBNO.Location = New System.Drawing.Point(179, 182)
        Me.txtDMOBNO.MaxLength = 1000
        Me.txtDMOBNO.Name = "txtDMOBNO"
        Me.txtDMOBNO.Size = New System.Drawing.Size(243, 26)
        Me.txtDMOBNO.TabIndex = 295
        '
        'txtEmoji
        '
        Me.txtEmoji.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmoji.Location = New System.Drawing.Point(179, 223)
        Me.txtEmoji.MaxLength = 1000
        Me.txtEmoji.Name = "txtEmoji"
        Me.txtEmoji.Size = New System.Drawing.Size(243, 26)
        Me.txtEmoji.TabIndex = 296
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(11, 229)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 16)
        Me.Label4.TabIndex = 297
        Me.Label4.Text = "Add Emoji"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(176, 263)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(203, 16)
        Me.Label5.TabIndex = 298
        Me.Label5.Text = "Press Windows + . to Add Emojis"
        '
        'frmUtlWAS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(664, 305)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtEmoji)
        Me.Controls.Add(Me.txtDMOBNO)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtMsgFileAPI)
        Me.Controls.Add(Me.txtMSGAPI)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnGetAPI)
        Me.Controls.Add(Me.btnClose)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtlWAS"
        Me.ShowIcon = False
        Me.Text = "WhatsApp Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnClose As Button
    Friend WithEvents btnGetAPI As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtMSGAPI As TextBox
    Friend WithEvents txtMsgFileAPI As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtDMOBNO As TextBox
    Friend WithEvents txtEmoji As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
End Class
