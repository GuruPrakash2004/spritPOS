<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlReg
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
        Me.lblRegNo = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtKey = New System.Windows.Forms.TextBox()
        Me.btnRegister = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(48, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Registration No"
        '
        'lblRegNo
        '
        Me.lblRegNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lblRegNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblRegNo.Font = New System.Drawing.Font("Bookman Old Style", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRegNo.Location = New System.Drawing.Point(134, 11)
        Me.lblRegNo.Name = "lblRegNo"
        Me.lblRegNo.Size = New System.Drawing.Size(158, 53)
        Me.lblRegNo.TabIndex = 1
        Me.lblRegNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(83, 91)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Key No."
        '
        'txtKey
        '
        Me.txtKey.BackColor = System.Drawing.Color.Yellow
        Me.txtKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtKey.Font = New System.Drawing.Font("Bookman Old Style", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtKey.Location = New System.Drawing.Point(134, 81)
        Me.txtKey.Name = "txtKey"
        Me.txtKey.Size = New System.Drawing.Size(155, 42)
        Me.txtKey.TabIndex = 3
        Me.txtKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnRegister
        '
        Me.btnRegister.Location = New System.Drawing.Point(23, 149)
        Me.btnRegister.Name = "btnRegister"
        Me.btnRegister.Size = New System.Drawing.Size(105, 28)
        Me.btnRegister.TabIndex = 4
        Me.btnRegister.Text = "Register"
        Me.btnRegister.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(296, 149)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(105, 28)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,95,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Location = New System.Drawing.Point(295, 39)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 19
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(114, 75)
        Me.vsfDet.TabIndex = 50
        Me.vsfDet.Visible = False
        '
        'frmUtlReg
        '
        Me.AcceptButton = Me.btnRegister
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(401, 189)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnRegister)
        Me.Controls.Add(Me.txtKey)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblRegNo)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmUtlReg"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Register Software"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblRegNo As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtKey As System.Windows.Forms.TextBox
    Friend WithEvents btnRegister As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
End Class
