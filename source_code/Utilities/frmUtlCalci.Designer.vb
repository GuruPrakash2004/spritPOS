<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlCalci
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlCalci))
        Me.txtFormula = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtResult = New System.Windows.Forms.TextBox()
        Me.lblError = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtFormula
        '
        Me.txtFormula.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtFormula.Location = New System.Drawing.Point(0, 0)
        Me.txtFormula.Margin = New System.Windows.Forms.Padding(5)
        Me.txtFormula.Multiline = True
        Me.txtFormula.Name = "txtFormula"
        Me.txtFormula.Size = New System.Drawing.Size(630, 97)
        Me.txtFormula.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Maroon
        Me.Label1.Location = New System.Drawing.Point(0, 100)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(352, 54)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Type the numbers and operators above." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Esc to Exit. Result will be copied in clip" &
    "board."
        '
        'txtResult
        '
        Me.txtResult.BackColor = System.Drawing.Color.Black
        Me.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtResult.Font = New System.Drawing.Font("Bookman Old Style", 25.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtResult.ForeColor = System.Drawing.Color.Lime
        Me.txtResult.Location = New System.Drawing.Point(354, 100)
        Me.txtResult.Name = "txtResult"
        Me.txtResult.ReadOnly = True
        Me.txtResult.Size = New System.Drawing.Size(276, 49)
        Me.txtResult.TabIndex = 2
        Me.txtResult.TabStop = False
        Me.txtResult.Text = "100"
        Me.txtResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblError
        '
        Me.lblError.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblError.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblError.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblError.ForeColor = System.Drawing.Color.Navy
        Me.lblError.Location = New System.Drawing.Point(0, 154)
        Me.lblError.Margin = New System.Windows.Forms.Padding(5)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(630, 43)
        Me.lblError.TabIndex = 4
        '
        'frmUtlCalci
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 197)
        Me.Controls.Add(Me.lblError)
        Me.Controls.Add(Me.txtResult)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtFormula)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtlCalci"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Simple Calci"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtFormula As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtResult As TextBox
    Friend WithEvents lblError As Label
End Class
