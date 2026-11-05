<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlInvSMS
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
        Me.pnlSMS = New System.Windows.Forms.Panel()
        Me.txtDelMsg = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtCancelMsg = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSalesMsg = New System.Windows.Forms.TextBox()
        Me.chkForDeletion = New System.Windows.Forms.CheckBox()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.chkForCancellation = New System.Windows.Forms.CheckBox()
        Me.txtValue = New System.Windows.Forms.TextBox()
        Me.lblValue = New System.Windows.Forms.Label()
        Me.chkForSalesValue = New System.Windows.Forms.CheckBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMobNo = New System.Windows.Forms.TextBox()
        Me.pnlSMS.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlSMS
        '
        Me.pnlSMS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSMS.Controls.Add(Me.txtDelMsg)
        Me.pnlSMS.Controls.Add(Me.Label7)
        Me.pnlSMS.Controls.Add(Me.txtCancelMsg)
        Me.pnlSMS.Controls.Add(Me.Label6)
        Me.pnlSMS.Controls.Add(Me.txtSalesMsg)
        Me.pnlSMS.Controls.Add(Me.chkForDeletion)
        Me.pnlSMS.Controls.Add(Me.lblMessage)
        Me.pnlSMS.Controls.Add(Me.chkForCancellation)
        Me.pnlSMS.Controls.Add(Me.txtValue)
        Me.pnlSMS.Controls.Add(Me.lblValue)
        Me.pnlSMS.Controls.Add(Me.chkForSalesValue)
        Me.pnlSMS.Location = New System.Drawing.Point(6, 7)
        Me.pnlSMS.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.pnlSMS.Name = "pnlSMS"
        Me.pnlSMS.Size = New System.Drawing.Size(673, 317)
        Me.pnlSMS.TabIndex = 90
        '
        'txtDelMsg
        '
        Me.txtDelMsg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDelMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDelMsg.Location = New System.Drawing.Point(117, 232)
        Me.txtDelMsg.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.txtDelMsg.MaxLength = 250
        Me.txtDelMsg.Multiline = True
        Me.txtDelMsg.Name = "txtDelMsg"
        Me.txtDelMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDelMsg.Size = New System.Drawing.Size(554, 54)
        Me.txtDelMsg.TabIndex = 7
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(62, 251)
        Me.Label7.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(50, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Message"
        '
        'txtCancelMsg
        '
        Me.txtCancelMsg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtCancelMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCancelMsg.Location = New System.Drawing.Point(117, 141)
        Me.txtCancelMsg.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.txtCancelMsg.MaxLength = 250
        Me.txtCancelMsg.Multiline = True
        Me.txtCancelMsg.Name = "txtCancelMsg"
        Me.txtCancelMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCancelMsg.Size = New System.Drawing.Size(554, 54)
        Me.txtCancelMsg.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(62, 158)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(50, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Message"
        '
        'txtSalesMsg
        '
        Me.txtSalesMsg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtSalesMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSalesMsg.Location = New System.Drawing.Point(117, 51)
        Me.txtSalesMsg.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.txtSalesMsg.MaxLength = 250
        Me.txtSalesMsg.Multiline = True
        Me.txtSalesMsg.Name = "txtSalesMsg"
        Me.txtSalesMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtSalesMsg.Size = New System.Drawing.Size(554, 54)
        Me.txtSalesMsg.TabIndex = 3
        '
        'chkForDeletion
        '
        Me.chkForDeletion.AutoSize = True
        Me.chkForDeletion.Location = New System.Drawing.Point(10, 214)
        Me.chkForDeletion.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkForDeletion.Name = "chkForDeletion"
        Me.chkForDeletion.Size = New System.Drawing.Size(109, 17)
        Me.chkForDeletion.TabIndex = 3
        Me.chkForDeletion.Text = "SMS For Deletion"
        Me.chkForDeletion.UseVisualStyleBackColor = True
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Location = New System.Drawing.Point(62, 67)
        Me.lblMessage.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(50, 13)
        Me.lblMessage.TabIndex = 2
        Me.lblMessage.Text = "Message"
        '
        'chkForCancellation
        '
        Me.chkForCancellation.AutoSize = True
        Me.chkForCancellation.Location = New System.Drawing.Point(10, 119)
        Me.chkForCancellation.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkForCancellation.Name = "chkForCancellation"
        Me.chkForCancellation.Size = New System.Drawing.Size(128, 17)
        Me.chkForCancellation.TabIndex = 2
        Me.chkForCancellation.Text = "SMS For Cancellation"
        Me.chkForCancellation.UseVisualStyleBackColor = True
        '
        'txtValue
        '
        Me.txtValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtValue.Location = New System.Drawing.Point(117, 20)
        Me.txtValue.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.txtValue.MaxLength = 20
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New System.Drawing.Size(164, 24)
        Me.txtValue.TabIndex = 1
        '
        'lblValue
        '
        Me.lblValue.AutoSize = True
        Me.lblValue.Location = New System.Drawing.Point(62, 22)
        Me.lblValue.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.Size = New System.Drawing.Size(43, 13)
        Me.lblValue.TabIndex = 0
        Me.lblValue.Text = "Value >"
        '
        'chkForSalesValue
        '
        Me.chkForSalesValue.AutoSize = True
        Me.chkForSalesValue.Location = New System.Drawing.Point(10, 2)
        Me.chkForSalesValue.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkForSalesValue.Name = "chkForSalesValue"
        Me.chkForSalesValue.Size = New System.Drawing.Size(131, 17)
        Me.chkForSalesValue.TabIndex = 1
        Me.chkForSalesValue.Text = "SMS For Sales Values"
        Me.chkForSalesValue.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(598, 330)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(81, 35)
        Me.btnClose.TabIndex = 92
        Me.btnClose.Text = "Cancel"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(500, 330)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(81, 35)
        Me.btnSave.TabIndex = 91
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 327)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(408, 13)
        Me.Label1.TabIndex = 93
        Me.Label1.Text = "Send Messages to Mobile  (Multiple Numbers can Add ex: 9880606365,9945036934)"
        '
        'txtMobNo
        '
        Me.txtMobNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtMobNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMobNo.Location = New System.Drawing.Point(11, 345)
        Me.txtMobNo.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.txtMobNo.MaxLength = 500
        Me.txtMobNo.Multiline = True
        Me.txtMobNo.Name = "txtMobNo"
        Me.txtMobNo.Size = New System.Drawing.Size(426, 28)
        Me.txtMobNo.TabIndex = 94
        '
        'frmUtlInvSMS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(702, 377)
        Me.Controls.Add(Me.txtMobNo)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.pnlSMS)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "frmUtlInvSMS"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Invoice - SMS Settings"
        Me.pnlSMS.ResumeLayout(False)
        Me.pnlSMS.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlSMS As Panel
    Friend WithEvents txtDelMsg As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtCancelMsg As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtSalesMsg As TextBox
    Friend WithEvents chkForDeletion As CheckBox
    Friend WithEvents lblMessage As Label
    Friend WithEvents chkForCancellation As CheckBox
    Friend WithEvents txtValue As TextBox
    Friend WithEvents lblValue As Label
    Friend WithEvents chkForSalesValue As CheckBox
    Friend WithEvents btnClose As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtMobNo As TextBox
End Class
