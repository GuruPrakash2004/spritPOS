<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrnRemoveDuplicates
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnRemoveDuplicates))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtRemove = New System.Windows.Forms.TextBox()
        Me.txtReplace = New System.Windows.Forms.TextBox()
        Me.btnRem = New System.Windows.Forms.Button()
        Me.btnRepl = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(51, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(147, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Item to Remove"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(51, 139)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(191, 25)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Item to Replace With"
        '
        'txtRemove
        '
        Me.txtRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemove.Location = New System.Drawing.Point(286, 54)
        Me.txtRemove.Name = "txtRemove"
        Me.txtRemove.Size = New System.Drawing.Size(451, 30)
        Me.txtRemove.TabIndex = 2
        '
        'txtReplace
        '
        Me.txtReplace.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReplace.Location = New System.Drawing.Point(286, 136)
        Me.txtReplace.Name = "txtReplace"
        Me.txtReplace.Size = New System.Drawing.Size(451, 30)
        Me.txtReplace.TabIndex = 3
        '
        'btnRem
        '
        Me.btnRem.Location = New System.Drawing.Point(743, 52)
        Me.btnRem.Name = "btnRem"
        Me.btnRem.Size = New System.Drawing.Size(49, 34)
        Me.btnRem.TabIndex = 4
        Me.btnRem.Text = "..."
        Me.btnRem.UseVisualStyleBackColor = True
        '
        'btnRepl
        '
        Me.btnRepl.Location = New System.Drawing.Point(743, 130)
        Me.btnRepl.Name = "btnRepl"
        Me.btnRepl.Size = New System.Drawing.Size(49, 34)
        Me.btnRepl.TabIndex = 5
        Me.btnRepl.Text = "..."
        Me.btnRepl.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdate.Location = New System.Drawing.Point(357, 216)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(282, 60)
        Me.btnUpdate.TabIndex = 6
        Me.btnUpdate.Text = "Merge and Remove First item"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(705, 216)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(146, 60)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmTrnRemoveDuplicates
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(889, 316)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnRepl)
        Me.Controls.Add(Me.btnRem)
        Me.Controls.Add(Me.txtReplace)
        Me.Controls.Add(Me.txtRemove)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmTrnRemoveDuplicates"
        Me.Text = "Merge and Remove Duplicate Item"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtRemove As TextBox
    Friend WithEvents txtReplace As TextBox
    Friend WithEvents btnRem As Button
    Friend WithEvents btnRepl As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnClose As Button
End Class
