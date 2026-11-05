<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlProfitLoss
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlProfitLoss))
        Me.lblVehicleNo = New System.Windows.Forms.Label()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.lblAmount = New System.Windows.Forms.Label()
        Me.lblCustName = New System.Windows.Forms.Label()
        Me.lblDifference = New System.Windows.Forms.Label()
        Me.lblReturnAmt = New System.Windows.Forms.Label()
        Me.txtVehicleNo = New System.Windows.Forms.TextBox()
        Me.txtDate = New System.Windows.Forms.TextBox()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.txtCustName = New System.Windows.Forms.TextBox()
        Me.txtRetAmt = New System.Windows.Forms.TextBox()
        Me.txtDifference = New System.Windows.Forms.TextBox()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.chkReturned = New System.Windows.Forms.CheckBox()
        Me.btnSelVehicle = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtSNo = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'lblVehicleNo
        '
        Me.lblVehicleNo.AutoSize = True
        Me.lblVehicleNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVehicleNo.ForeColor = System.Drawing.Color.Blue
        Me.lblVehicleNo.Location = New System.Drawing.Point(12, 100)
        Me.lblVehicleNo.Name = "lblVehicleNo"
        Me.lblVehicleNo.Size = New System.Drawing.Size(121, 20)
        Me.lblVehicleNo.TabIndex = 0
        Me.lblVehicleNo.Tag = "R"
        Me.lblVehicleNo.Text = "Vehicle Number"
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.ForeColor = System.Drawing.Color.Blue
        Me.lblDate.Location = New System.Drawing.Point(12, 146)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(44, 20)
        Me.lblDate.TabIndex = 1
        Me.lblDate.Tag = "R"
        Me.lblDate.Text = "Date"
        '
        'lblAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAmount.ForeColor = System.Drawing.Color.Blue
        Me.lblAmount.Location = New System.Drawing.Point(12, 192)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.Size = New System.Drawing.Size(65, 20)
        Me.lblAmount.TabIndex = 2
        Me.lblAmount.Tag = "R"
        Me.lblAmount.Text = "Amount"
        '
        'lblCustName
        '
        Me.lblCustName.AutoSize = True
        Me.lblCustName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustName.ForeColor = System.Drawing.Color.Blue
        Me.lblCustName.Location = New System.Drawing.Point(12, 238)
        Me.lblCustName.Name = "lblCustName"
        Me.lblCustName.Size = New System.Drawing.Size(124, 20)
        Me.lblCustName.TabIndex = 3
        Me.lblCustName.Tag = "R"
        Me.lblCustName.Text = "Customer Name"
        '
        'lblDifference
        '
        Me.lblDifference.AutoSize = True
        Me.lblDifference.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDifference.ForeColor = System.Drawing.Color.Blue
        Me.lblDifference.Location = New System.Drawing.Point(12, 330)
        Me.lblDifference.Name = "lblDifference"
        Me.lblDifference.Size = New System.Drawing.Size(83, 20)
        Me.lblDifference.TabIndex = 4
        Me.lblDifference.Tag = "R"
        Me.lblDifference.Text = "Difference"
        '
        'lblReturnAmt
        '
        Me.lblReturnAmt.AutoSize = True
        Me.lblReturnAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReturnAmt.ForeColor = System.Drawing.Color.Red
        Me.lblReturnAmt.Location = New System.Drawing.Point(12, 284)
        Me.lblReturnAmt.Name = "lblReturnAmt"
        Me.lblReturnAmt.Size = New System.Drawing.Size(154, 20)
        Me.lblReturnAmt.TabIndex = 5
        Me.lblReturnAmt.Tag = "M"
        Me.lblReturnAmt.Text = "Return Patti Amount"
        '
        'txtVehicleNo
        '
        Me.txtVehicleNo.BackColor = System.Drawing.Color.White
        Me.txtVehicleNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVehicleNo.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.txtVehicleNo.Location = New System.Drawing.Point(183, 97)
        Me.txtVehicleNo.MaxLength = 50
        Me.txtVehicleNo.Name = "txtVehicleNo"
        Me.txtVehicleNo.ReadOnly = True
        Me.txtVehicleNo.Size = New System.Drawing.Size(306, 26)
        Me.txtVehicleNo.TabIndex = 230
        Me.txtVehicleNo.Tag = "R"
        '
        'txtDate
        '
        Me.txtDate.BackColor = System.Drawing.Color.White
        Me.txtDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDate.Location = New System.Drawing.Point(183, 143)
        Me.txtDate.MaxLength = 50
        Me.txtDate.Name = "txtDate"
        Me.txtDate.ReadOnly = True
        Me.txtDate.Size = New System.Drawing.Size(306, 26)
        Me.txtDate.TabIndex = 231
        Me.txtDate.Tag = "R"
        '
        'txtAmount
        '
        Me.txtAmount.BackColor = System.Drawing.Color.White
        Me.txtAmount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.Location = New System.Drawing.Point(183, 189)
        Me.txtAmount.MaxLength = 50
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.ReadOnly = True
        Me.txtAmount.Size = New System.Drawing.Size(306, 26)
        Me.txtAmount.TabIndex = 232
        Me.txtAmount.Tag = "R"
        '
        'txtCustName
        '
        Me.txtCustName.BackColor = System.Drawing.Color.White
        Me.txtCustName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCustName.Location = New System.Drawing.Point(183, 235)
        Me.txtCustName.MaxLength = 250
        Me.txtCustName.Name = "txtCustName"
        Me.txtCustName.ReadOnly = True
        Me.txtCustName.Size = New System.Drawing.Size(306, 26)
        Me.txtCustName.TabIndex = 233
        Me.txtCustName.Tag = "R"
        '
        'txtRetAmt
        '
        Me.txtRetAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRetAmt.Location = New System.Drawing.Point(183, 281)
        Me.txtRetAmt.MaxLength = 50
        Me.txtRetAmt.Name = "txtRetAmt"
        Me.txtRetAmt.Size = New System.Drawing.Size(306, 26)
        Me.txtRetAmt.TabIndex = 234
        '
        'txtDifference
        '
        Me.txtDifference.BackColor = System.Drawing.Color.White
        Me.txtDifference.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDifference.Location = New System.Drawing.Point(183, 327)
        Me.txtDifference.MaxLength = 50
        Me.txtDifference.Name = "txtDifference"
        Me.txtDifference.ReadOnly = True
        Me.txtDifference.Size = New System.Drawing.Size(306, 26)
        Me.txtDifference.TabIndex = 235
        Me.txtDifference.Tag = "R"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vFactIC.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(474, 397)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(92, 31)
        Me.btnCancel.TabIndex = 243
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vFactIC.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.Black
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(384, 397)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(82, 31)
        Me.btnSave.TabIndex = 242
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'chkReturned
        '
        Me.chkReturned.AutoSize = True
        Me.chkReturned.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReturned.ForeColor = System.Drawing.Color.Black
        Me.chkReturned.Location = New System.Drawing.Point(183, 66)
        Me.chkReturned.Margin = New System.Windows.Forms.Padding(4)
        Me.chkReturned.Name = "chkReturned"
        Me.chkReturned.Size = New System.Drawing.Size(144, 21)
        Me.chkReturned.TabIndex = 244
        Me.chkReturned.Text = "Show Returned Also"
        Me.chkReturned.UseVisualStyleBackColor = True
        '
        'btnSelVehicle
        '
        Me.btnSelVehicle.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelVehicle.ForeColor = System.Drawing.Color.Black
        Me.btnSelVehicle.Location = New System.Drawing.Point(495, 100)
        Me.btnSelVehicle.Name = "btnSelVehicle"
        Me.btnSelVehicle.Size = New System.Drawing.Size(32, 23)
        Me.btnSelVehicle.TabIndex = 245
        Me.btnSelVehicle.Text = "..."
        Me.btnSelVehicle.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(12, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(159, 20)
        Me.Label1.TabIndex = 246
        Me.Label1.Tag = ""
        Me.Label1.Text = "Return Amount Entry"
        '
        'txtSNo
        '
        Me.txtSNo.Location = New System.Drawing.Point(528, 12)
        Me.txtSNo.Name = "txtSNo"
        Me.txtSNo.Size = New System.Drawing.Size(41, 20)
        Me.txtSNo.TabIndex = 247
        Me.txtSNo.Visible = False
        '
        'frmUtlProfitLoss
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(579, 441)
        Me.Controls.Add(Me.txtSNo)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSelVehicle)
        Me.Controls.Add(Me.chkReturned)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtDifference)
        Me.Controls.Add(Me.txtRetAmt)
        Me.Controls.Add(Me.txtCustName)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.txtDate)
        Me.Controls.Add(Me.txtVehicleNo)
        Me.Controls.Add(Me.lblReturnAmt)
        Me.Controls.Add(Me.lblDifference)
        Me.Controls.Add(Me.lblCustName)
        Me.Controls.Add(Me.lblAmount)
        Me.Controls.Add(Me.lblDate)
        Me.Controls.Add(Me.lblVehicleNo)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Blue
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmUtlProfitLoss"
        Me.Text = "Profit And Loss"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblVehicleNo As Label
    Friend WithEvents lblDate As Label
    Friend WithEvents lblAmount As Label
    Friend WithEvents lblCustName As Label
    Friend WithEvents lblDifference As Label
    Friend WithEvents lblReturnAmt As Label
    Friend WithEvents txtVehicleNo As TextBox
    Friend WithEvents txtDate As TextBox
    Friend WithEvents txtAmount As TextBox
    Friend WithEvents txtCustName As TextBox
    Friend WithEvents txtRetAmt As TextBox
    Friend WithEvents txtDifference As TextBox
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents chkReturned As CheckBox
    Friend WithEvents btnSelVehicle As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtSNo As TextBox
    Friend WithEvents ToolTip1 As ToolTip
End Class
