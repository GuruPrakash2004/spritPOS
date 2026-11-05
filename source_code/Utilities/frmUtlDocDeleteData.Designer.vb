<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlDocDeleteData
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.RboVouchers = New System.Windows.Forms.RadioButton()
        Me.RboTransactions = New System.Windows.Forms.RadioButton()
        Me.RboMasters = New System.Windows.Forms.RadioButton()
        Me.vsfDET = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.BtnDlt = New System.Windows.Forms.Button()
        Me.chkSelClr = New System.Windows.Forms.CheckBox()
        Me.Panel1.SuspendLayout()
        CType(Me.vsfDET, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.RboVouchers)
        Me.Panel1.Controls.Add(Me.RboTransactions)
        Me.Panel1.Controls.Add(Me.RboMasters)
        Me.Panel1.Location = New System.Drawing.Point(13, 13)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(306, 38)
        Me.Panel1.TabIndex = 0
        '
        'RboVouchers
        '
        Me.RboVouchers.AutoSize = True
        Me.RboVouchers.Location = New System.Drawing.Point(223, 13)
        Me.RboVouchers.Name = "RboVouchers"
        Me.RboVouchers.Size = New System.Drawing.Size(70, 17)
        Me.RboVouchers.TabIndex = 2
        Me.RboVouchers.TabStop = True
        Me.RboVouchers.Text = "Vouchers"
        Me.RboVouchers.UseVisualStyleBackColor = True
        '
        'RboTransactions
        '
        Me.RboTransactions.AutoSize = True
        Me.RboTransactions.Location = New System.Drawing.Point(112, 13)
        Me.RboTransactions.Name = "RboTransactions"
        Me.RboTransactions.Size = New System.Drawing.Size(86, 17)
        Me.RboTransactions.TabIndex = 1
        Me.RboTransactions.TabStop = True
        Me.RboTransactions.Text = "Transactions"
        Me.RboTransactions.UseVisualStyleBackColor = True
        '
        'RboMasters
        '
        Me.RboMasters.AutoSize = True
        Me.RboMasters.Location = New System.Drawing.Point(25, 13)
        Me.RboMasters.Name = "RboMasters"
        Me.RboMasters.Size = New System.Drawing.Size(62, 17)
        Me.RboMasters.TabIndex = 0
        Me.RboMasters.TabStop = True
        Me.RboMasters.Text = "Masters"
        Me.RboMasters.UseVisualStyleBackColor = True
        '
        'vsfDET
        '
        Me.vsfDET.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDET.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDET.AutoResize = True
        Me.vsfDET.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDET.ColumnInfo = "10,0,0,0,0,120,Columns:0{Width:70;}" & Global.Microsoft.VisualBasic.ChrW(9) & "1{Width:164;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDET.ExtendLastCol = True
        Me.vsfDET.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.vsfDET.Location = New System.Drawing.Point(13, 59)
        Me.vsfDET.Margin = New System.Windows.Forms.Padding(5)
        Me.vsfDET.Name = "vsfDET"
        Me.vsfDET.Rows.DefaultSize = 24
        Me.vsfDET.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfDET.Size = New System.Drawing.Size(630, 393)
        Me.vsfDET.TabIndex = 229
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.Transparent
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Location = New System.Drawing.Point(575, 460)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(68, 29)
        Me.btnClose.TabIndex = 230
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'BtnDlt
        '
        Me.BtnDlt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnDlt.BackColor = System.Drawing.Color.Transparent
        Me.BtnDlt.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.BtnDlt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnDlt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDlt.Location = New System.Drawing.Point(466, 460)
        Me.BtnDlt.Name = "BtnDlt"
        Me.BtnDlt.Size = New System.Drawing.Size(100, 29)
        Me.BtnDlt.TabIndex = 231
        Me.BtnDlt.Text = "Delete Data"
        Me.BtnDlt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnDlt.UseVisualStyleBackColor = False
        '
        'chkSelClr
        '
        Me.chkSelClr.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSelClr.AutoSize = True
        Me.chkSelClr.Location = New System.Drawing.Point(13, 472)
        Me.chkSelClr.Name = "chkSelClr"
        Me.chkSelClr.Size = New System.Drawing.Size(99, 17)
        Me.chkSelClr.TabIndex = 233
        Me.chkSelClr.Text = "Select/Clear All"
        Me.chkSelClr.UseVisualStyleBackColor = False
        '
        'frmUtlDocDeleteData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(655, 501)
        Me.Controls.Add(Me.chkSelClr)
        Me.Controls.Add(Me.BtnDlt)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfDET)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmUtlDocDeleteData"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Document Delete Data"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.vsfDET, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents RboVouchers As RadioButton
    Friend WithEvents RboTransactions As RadioButton
    Friend WithEvents RboMasters As RadioButton
    Friend WithEvents vsfDET As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As Button
    Friend WithEvents BtnDlt As Button
    Friend WithEvents chkSelClr As CheckBox
End Class
