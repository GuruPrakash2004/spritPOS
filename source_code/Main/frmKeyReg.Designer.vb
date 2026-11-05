<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmKeyReg
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtRegNo = New System.Windows.Forms.TextBox()
        Me.txtKey = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(62, 71)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(99, 26)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Reg. No"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(62, 120)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 26)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Key"
        '
        'txtRegNo
        '
        Me.txtRegNo.BackColor = System.Drawing.Color.White
        Me.txtRegNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRegNo.Location = New System.Drawing.Point(190, 66)
        Me.txtRegNo.Margin = New System.Windows.Forms.Padding(2)
        Me.txtRegNo.MaxLength = 5
        Me.txtRegNo.Name = "txtRegNo"
        Me.txtRegNo.Size = New System.Drawing.Size(165, 32)
        Me.txtRegNo.TabIndex = 2
        '
        'txtKey
        '
        Me.txtKey.BackColor = System.Drawing.Color.White
        Me.txtKey.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKey.Location = New System.Drawing.Point(190, 115)
        Me.txtKey.Margin = New System.Windows.Forms.Padding(2)
        Me.txtKey.Name = "txtKey"
        Me.txtKey.Size = New System.Drawing.Size(165, 32)
        Me.txtKey.TabIndex = 3
        '
        'frmKeyReg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(433, 223)
        Me.Controls.Add(Me.txtKey)
        Me.Controls.Add(Me.txtRegNo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximumSize = New System.Drawing.Size(449, 261)
        Me.Name = "frmKeyReg"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Key Generator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtRegNo As TextBox
    Friend WithEvents txtKey As TextBox
End Class
