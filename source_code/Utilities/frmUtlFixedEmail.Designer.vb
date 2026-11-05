<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlFixedEmail
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlFixedEmail))
        Me.gbEditEMail = New System.Windows.Forms.GroupBox()
        Me.btnPnlClose = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.pnlVariable = New System.Windows.Forms.Panel()
        Me.txtFldCond = New System.Windows.Forms.TextBox()
        Me.txtFieldName = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.pnlFixed = New System.Windows.Forms.Panel()
        Me.txtFixedEMailID = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cboSendingType = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rdoVariable = New System.Windows.Forms.RadioButton()
        Me.rdoFixed = New System.Windows.Forms.RadioButton()
        Me.gbEditEMail.SuspendLayout()
        Me.pnlVariable.SuspendLayout()
        Me.pnlFixed.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbEditEMail
        '
        Me.gbEditEMail.BackColor = System.Drawing.Color.Transparent
        Me.gbEditEMail.Controls.Add(Me.btnPnlClose)
        Me.gbEditEMail.Controls.Add(Me.btnOK)
        Me.gbEditEMail.Controls.Add(Me.pnlVariable)
        Me.gbEditEMail.Controls.Add(Me.pnlFixed)
        Me.gbEditEMail.Controls.Add(Me.cboSendingType)
        Me.gbEditEMail.Controls.Add(Me.Label4)
        Me.gbEditEMail.Controls.Add(Me.Panel2)
        Me.gbEditEMail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbEditEMail.Location = New System.Drawing.Point(0, 0)
        Me.gbEditEMail.Name = "gbEditEMail"
        Me.gbEditEMail.Size = New System.Drawing.Size(731, 260)
        Me.gbEditEMail.TabIndex = 16
        Me.gbEditEMail.TabStop = False
        '
        'btnPnlClose
        '
        Me.btnPnlClose.Location = New System.Drawing.Point(626, 219)
        Me.btnPnlClose.Name = "btnPnlClose"
        Me.btnPnlClose.Size = New System.Drawing.Size(88, 31)
        Me.btnPnlClose.TabIndex = 209
        Me.btnPnlClose.Text = "Cancel"
        Me.btnPnlClose.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(532, 219)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(88, 31)
        Me.btnOK.TabIndex = 208
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'pnlVariable
        '
        Me.pnlVariable.Controls.Add(Me.txtFldCond)
        Me.pnlVariable.Controls.Add(Me.txtFieldName)
        Me.pnlVariable.Controls.Add(Me.Label10)
        Me.pnlVariable.Controls.Add(Me.Label9)
        Me.pnlVariable.Location = New System.Drawing.Point(131, 107)
        Me.pnlVariable.Name = "pnlVariable"
        Me.pnlVariable.Size = New System.Drawing.Size(591, 106)
        Me.pnlVariable.TabIndex = 8
        Me.pnlVariable.Visible = False
        '
        'txtFldCond
        '
        Me.txtFldCond.Location = New System.Drawing.Point(143, 38)
        Me.txtFldCond.Multiline = True
        Me.txtFldCond.Name = "txtFldCond"
        Me.txtFldCond.Size = New System.Drawing.Size(438, 65)
        Me.txtFldCond.TabIndex = 15
        '
        'txtFieldName
        '
        Me.txtFieldName.Location = New System.Drawing.Point(143, 6)
        Me.txtFieldName.Name = "txtFieldName"
        Me.txtFieldName.Size = New System.Drawing.Size(438, 26)
        Me.txtFieldName.TabIndex = 14
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 38)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(114, 20)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Field Condition"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(7, 9)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(89, 20)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "Field Name"
        '
        'pnlFixed
        '
        Me.pnlFixed.Controls.Add(Me.txtFixedEMailID)
        Me.pnlFixed.Controls.Add(Me.Label8)
        Me.pnlFixed.Location = New System.Drawing.Point(131, 64)
        Me.pnlFixed.Name = "pnlFixed"
        Me.pnlFixed.Size = New System.Drawing.Size(590, 37)
        Me.pnlFixed.TabIndex = 7
        '
        'txtFixedEMailID
        '
        Me.txtFixedEMailID.Location = New System.Drawing.Point(143, 3)
        Me.txtFixedEMailID.Name = "txtFixedEMailID"
        Me.txtFixedEMailID.Size = New System.Drawing.Size(438, 26)
        Me.txtFixedEMailID.TabIndex = 13
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(5, 8)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(67, 20)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "eMail ID"
        '
        'cboSendingType
        '
        Me.cboSendingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSendingType.FormattingEnabled = True
        Me.cboSendingType.Items.AddRange(New Object() {"", "To", "Cc", "Bcc"})
        Me.cboSendingType.Location = New System.Drawing.Point(130, 25)
        Me.cboSendingType.Name = "cboSendingType"
        Me.cboSendingType.Size = New System.Drawing.Size(121, 28)
        Me.cboSendingType.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 31)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(106, 20)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Sending Type"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.rdoVariable)
        Me.Panel2.Controls.Add(Me.rdoFixed)
        Me.Panel2.Location = New System.Drawing.Point(10, 64)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(107, 61)
        Me.Panel2.TabIndex = 0
        '
        'rdoVariable
        '
        Me.rdoVariable.AutoSize = True
        Me.rdoVariable.Location = New System.Drawing.Point(7, 30)
        Me.rdoVariable.Name = "rdoVariable"
        Me.rdoVariable.Size = New System.Drawing.Size(85, 24)
        Me.rdoVariable.TabIndex = 1
        Me.rdoVariable.Text = "Variable"
        Me.rdoVariable.UseVisualStyleBackColor = True
        '
        'rdoFixed
        '
        Me.rdoFixed.AutoSize = True
        Me.rdoFixed.Checked = True
        Me.rdoFixed.Location = New System.Drawing.Point(7, 5)
        Me.rdoFixed.Name = "rdoFixed"
        Me.rdoFixed.Size = New System.Drawing.Size(65, 24)
        Me.rdoFixed.TabIndex = 0
        Me.rdoFixed.TabStop = True
        Me.rdoFixed.Text = "Fixed"
        Me.rdoFixed.UseVisualStyleBackColor = True
        '
        'frmUtlFixedEmail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(731, 260)
        Me.Controls.Add(Me.gbEditEMail)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmUtlFixedEmail"
        Me.gbEditEMail.ResumeLayout(False)
        Me.gbEditEMail.PerformLayout()
        Me.pnlVariable.ResumeLayout(False)
        Me.pnlVariable.PerformLayout()
        Me.pnlFixed.ResumeLayout(False)
        Me.pnlFixed.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnPnlClose As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents pnlVariable As Panel
    Friend WithEvents txtFldCond As TextBox
    Friend WithEvents txtFieldName As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents pnlFixed As Panel
    Friend WithEvents txtFixedEMailID As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cboSendingType As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents rdoVariable As RadioButton
    Friend WithEvents rdoFixed As RadioButton
    Public WithEvents gbEditEMail As GroupBox
End Class
