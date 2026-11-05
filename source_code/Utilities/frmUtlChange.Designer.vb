<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlChange
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
        Me.txtBillID = New System.Windows.Forms.TextBox()
        Me.txtBillNo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnPrevBill = New System.Windows.Forms.Button()
        Me.btnNextBill = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtBillAmt = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtRcvdAmt = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPayBack = New System.Windows.Forms.TextBox()
        Me.btnAutoRcv = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtBillID
        '
        Me.txtBillID.Location = New System.Drawing.Point(49, 224)
        Me.txtBillID.Name = "txtBillID"
        Me.txtBillID.Size = New System.Drawing.Size(100, 20)
        Me.txtBillID.TabIndex = 0
        Me.txtBillID.Visible = False
        '
        'txtBillNo
        '
        Me.txtBillNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtBillNo.Font = New System.Drawing.Font("Bookman Old Style", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBillNo.Location = New System.Drawing.Point(123, 13)
        Me.txtBillNo.Name = "txtBillNo"
        Me.txtBillNo.ReadOnly = True
        Me.txtBillNo.Size = New System.Drawing.Size(213, 30)
        Me.txtBillNo.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Bookman Old Style", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(44, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 22)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Bill No"
        '
        'btnPrevBill
        '
        Me.btnPrevBill.Location = New System.Drawing.Point(351, 9)
        Me.btnPrevBill.Name = "btnPrevBill"
        Me.btnPrevBill.Size = New System.Drawing.Size(75, 23)
        Me.btnPrevBill.TabIndex = 3
        Me.btnPrevBill.Text = "Prev. Bill"
        Me.btnPrevBill.UseVisualStyleBackColor = True
        '
        'btnNextBill
        '
        Me.btnNextBill.Location = New System.Drawing.Point(349, 39)
        Me.btnNextBill.Name = "btnNextBill"
        Me.btnNextBill.Size = New System.Drawing.Size(75, 23)
        Me.btnNextBill.TabIndex = 4
        Me.btnNextBill.Text = "Next Bill"
        Me.btnNextBill.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Location = New System.Drawing.Point(-3, 66)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(428, 10)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Bookman Old Style", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(36, 87)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(126, 22)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Bill Amount"
        '
        'txtBillAmt
        '
        Me.txtBillAmt.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.txtBillAmt.Enabled = False
        Me.txtBillAmt.Font = New System.Drawing.Font("Bookman Old Style", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBillAmt.Location = New System.Drawing.Point(197, 86)
        Me.txtBillAmt.Name = "txtBillAmt"
        Me.txtBillAmt.ReadOnly = True
        Me.txtBillAmt.Size = New System.Drawing.Size(213, 39)
        Me.txtBillAmt.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Bookman Old Style", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(36, 128)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(148, 22)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Received Amt"
        '
        'txtRcvdAmt
        '
        Me.txtRcvdAmt.BackColor = System.Drawing.Color.Yellow
        Me.txtRcvdAmt.Font = New System.Drawing.Font("Bookman Old Style", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRcvdAmt.Location = New System.Drawing.Point(197, 129)
        Me.txtRcvdAmt.Name = "txtRcvdAmt"
        Me.txtRcvdAmt.Size = New System.Drawing.Size(213, 39)
        Me.txtRcvdAmt.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Bookman Old Style", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(36, 172)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(102, 22)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Pay Back"
        '
        'txtPayBack
        '
        Me.txtPayBack.BackColor = System.Drawing.Color.Cornsilk
        Me.txtPayBack.Font = New System.Drawing.Font("Bookman Old Style", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayBack.Location = New System.Drawing.Point(197, 170)
        Me.txtPayBack.Name = "txtPayBack"
        Me.txtPayBack.ReadOnly = True
        Me.txtPayBack.Size = New System.Drawing.Size(213, 39)
        Me.txtPayBack.TabIndex = 10
        '
        'btnAutoRcv
        '
        Me.btnAutoRcv.Location = New System.Drawing.Point(195, 219)
        Me.btnAutoRcv.Name = "btnAutoRcv"
        Me.btnAutoRcv.Size = New System.Drawing.Size(142, 37)
        Me.btnAutoRcv.TabIndex = 12
        Me.btnAutoRcv.Text = "Auto Rcvd Amt (Pg Up)"
        Me.btnAutoRcv.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(343, 221)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(65, 33)
        Me.btnOK.TabIndex = 13
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'frmUtlChange
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(432, 263)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnAutoRcv)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtPayBack)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtRcvdAmt)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtBillAmt)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnNextBill)
        Me.Controls.Add(Me.btnPrevBill)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtBillNo)
        Me.Controls.Add(Me.txtBillID)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmUtlChange"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tender Change..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtBillID As System.Windows.Forms.TextBox
    Friend WithEvents txtBillNo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnPrevBill As System.Windows.Forms.Button
    Friend WithEvents btnNextBill As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtBillAmt As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtRcvdAmt As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPayBack As System.Windows.Forms.TextBox
    Friend WithEvents btnAutoRcv As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
End Class
