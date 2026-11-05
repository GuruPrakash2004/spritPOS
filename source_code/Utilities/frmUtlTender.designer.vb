<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlTender
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlTender))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtBillAmt = New System.Windows.Forms.TextBox()
        Me.fraRcpt = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnAutoRcv = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtPayBack = New System.Windows.Forms.TextBox()
        Me.txtCash = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtCardAmt = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtRcvdAmt = New System.Windows.Forms.TextBox()
        Me.lblBillNo = New System.Windows.Forms.Label()
        Me.txtInvId = New System.Windows.Forms.TextBox()
        Me.fraRcpt.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(16, 68)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 21)
        Me.Label1.TabIndex = 58
        Me.Label1.Text = "Bill Amount"
        '
        'GroupBox1
        '
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(11, 52)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(428, 10)
        Me.GroupBox1.TabIndex = 60
        Me.GroupBox1.TabStop = False
        '
        'txtBillAmt
        '
        Me.txtBillAmt.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.txtBillAmt.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBillAmt.ForeColor = System.Drawing.Color.White
        Me.txtBillAmt.Location = New System.Drawing.Point(241, 65)
        Me.txtBillAmt.MaxLength = 10
        Me.txtBillAmt.Name = "txtBillAmt"
        Me.txtBillAmt.Size = New System.Drawing.Size(213, 33)
        Me.txtBillAmt.TabIndex = 61
        Me.txtBillAmt.Text = "0.0"
        Me.txtBillAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'fraRcpt
        '
        Me.fraRcpt.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraRcpt.Controls.Add(Me.btnClose)
        Me.fraRcpt.Controls.Add(Me.btnOK)
        Me.fraRcpt.Controls.Add(Me.btnAutoRcv)
        Me.fraRcpt.Controls.Add(Me.GroupBox2)
        Me.fraRcpt.Controls.Add(Me.Label9)
        Me.fraRcpt.Controls.Add(Me.txtPayBack)
        Me.fraRcpt.Controls.Add(Me.txtCash)
        Me.fraRcpt.Controls.Add(Me.Label5)
        Me.fraRcpt.Controls.Add(Me.txtCardAmt)
        Me.fraRcpt.Controls.Add(Me.Label3)
        Me.fraRcpt.Controls.Add(Me.Label8)
        Me.fraRcpt.Controls.Add(Me.txtRcvdAmt)
        Me.fraRcpt.Controls.Add(Me.lblBillNo)
        Me.fraRcpt.Controls.Add(Me.txtBillAmt)
        Me.fraRcpt.Controls.Add(Me.GroupBox1)
        Me.fraRcpt.Controls.Add(Me.Label1)
        Me.fraRcpt.Controls.Add(Me.txtInvId)
        Me.fraRcpt.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraRcpt.Location = New System.Drawing.Point(12, 16)
        Me.fraRcpt.Name = "fraRcpt"
        Me.fraRcpt.Size = New System.Drawing.Size(484, 481)
        Me.fraRcpt.TabIndex = 62
        '
        'btnClose
        '
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(325, 314)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(147, 50)
        Me.btnClose.TabIndex = 76
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(172, 311)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(147, 52)
        Me.btnOK.TabIndex = 73
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnAutoRcv
        '
        Me.btnAutoRcv.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAutoRcv.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAutoRcv.Location = New System.Drawing.Point(21, 311)
        Me.btnAutoRcv.Name = "btnAutoRcv"
        Me.btnAutoRcv.Size = New System.Drawing.Size(147, 52)
        Me.btnAutoRcv.TabIndex = 72
        Me.btnAutoRcv.Text = "A&uto Rcvd Amt (Pg Up)"
        Me.btnAutoRcv.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(11, 277)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(465, 16)
        Me.GroupBox2.TabIndex = 71
        Me.GroupBox2.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(16, 236)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(91, 25)
        Me.Label9.TabIndex = 70
        Me.Label9.Text = "Pay Back"
        '
        'txtPayBack
        '
        Me.txtPayBack.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.txtPayBack.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayBack.ForeColor = System.Drawing.Color.White
        Me.txtPayBack.Location = New System.Drawing.Point(241, 229)
        Me.txtPayBack.MaxLength = 10
        Me.txtPayBack.Name = "txtPayBack"
        Me.txtPayBack.ReadOnly = True
        Me.txtPayBack.Size = New System.Drawing.Size(213, 33)
        Me.txtPayBack.TabIndex = 69
        Me.txtPayBack.TabStop = False
        Me.txtPayBack.Text = "0.0"
        Me.txtPayBack.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCash
        '
        Me.txtCash.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.txtCash.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCash.ForeColor = System.Drawing.Color.White
        Me.txtCash.Location = New System.Drawing.Point(241, 147)
        Me.txtCash.MaxLength = 10
        Me.txtCash.Name = "txtCash"
        Me.txtCash.ReadOnly = True
        Me.txtCash.Size = New System.Drawing.Size(213, 29)
        Me.txtCash.TabIndex = 68
        Me.txtCash.TabStop = False
        Me.txtCash.Text = "0.0"
        Me.txtCash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(16, 194)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(103, 21)
        Me.Label5.TabIndex = 67
        Me.Label5.Text = "Card Amount"
        '
        'txtCardAmt
        '
        Me.txtCardAmt.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.txtCardAmt.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCardAmt.ForeColor = System.Drawing.Color.White
        Me.txtCardAmt.Location = New System.Drawing.Point(241, 188)
        Me.txtCardAmt.MaxLength = 10
        Me.txtCardAmt.Name = "txtCardAmt"
        Me.txtCardAmt.Size = New System.Drawing.Size(213, 29)
        Me.txtCardAmt.TabIndex = 66
        Me.txtCardAmt.Text = "0.0"
        Me.txtCardAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 152)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 21)
        Me.Label3.TabIndex = 65
        Me.Label3.Text = "Cash Amount"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(16, 110)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(132, 21)
        Me.Label8.TabIndex = 64
        Me.Label8.Text = "Received Amount"
        '
        'txtRcvdAmt
        '
        Me.txtRcvdAmt.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.txtRcvdAmt.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRcvdAmt.ForeColor = System.Drawing.Color.White
        Me.txtRcvdAmt.Location = New System.Drawing.Point(241, 106)
        Me.txtRcvdAmt.MaxLength = 10
        Me.txtRcvdAmt.Name = "txtRcvdAmt"
        Me.txtRcvdAmt.Size = New System.Drawing.Size(213, 29)
        Me.txtRcvdAmt.TabIndex = 63
        Me.txtRcvdAmt.Text = "0.0"
        Me.txtRcvdAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblBillNo
        '
        Me.lblBillNo.AutoSize = True
        Me.lblBillNo.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillNo.ForeColor = System.Drawing.Color.Black
        Me.lblBillNo.Location = New System.Drawing.Point(16, 24)
        Me.lblBillNo.Name = "lblBillNo"
        Me.lblBillNo.Size = New System.Drawing.Size(65, 25)
        Me.lblBillNo.TabIndex = 62
        Me.lblBillNo.Text = "Bill # :"
        '
        'txtInvId
        '
        Me.txtInvId.Location = New System.Drawing.Point(219, 334)
        Me.txtInvId.Name = "txtInvId"
        Me.txtInvId.Size = New System.Drawing.Size(100, 29)
        Me.txtInvId.TabIndex = 77
        '
        'frmUtlTender
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(508, 509)
        Me.Controls.Add(Me.fraRcpt)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmUtlTender"
        Me.Text = "Tender Change"
        Me.fraRcpt.ResumeLayout(False)
        Me.fraRcpt.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtBillAmt As System.Windows.Forms.TextBox
    Friend WithEvents fraRcpt As System.Windows.Forms.Panel
    Friend WithEvents lblBillNo As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtPayBack As System.Windows.Forms.TextBox
    Friend WithEvents txtCash As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtCardAmt As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtRcvdAmt As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnAutoRcv As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtInvId As System.Windows.Forms.TextBox
End Class
