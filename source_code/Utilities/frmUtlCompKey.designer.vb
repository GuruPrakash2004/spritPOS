<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlCompKey
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
        Dim CBlendItems1 As vICAxon.cBlendItems = New vICAxon.cBlendItems()
        Dim CBlendItems2 As vICAxon.cBlendItems = New vICAxon.cBlendItems()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlCompKey))
        Dim CBlendItems3 As vICAxon.cBlendItems = New vICAxon.cBlendItems()
        Me.txtNameKey = New System.Windows.Forms.TextBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtCmpName = New System.Windows.Forms.TextBox()
        Me.txtCmpAddr = New System.Windows.Forms.TextBox()
        Me.txtAddID = New System.Windows.Forms.TextBox()
        Me.txtCustID = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnedit = New vICAxon.CButton()
        Me.btnadd = New vICAxon.CButton()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnKB = New System.Windows.Forms.Button()
        Me.txtAddrKey = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnSaveKey = New C1.Win.C1Input.C1Button()
        Me.btnCancelKey = New C1.Win.C1Input.C1Button()
        Me.btnSend = New vICAxon.CButton()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtNameKey
        '
        Me.txtNameKey.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNameKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNameKey.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.txtNameKey.Location = New System.Drawing.Point(114, 28)
        Me.txtNameKey.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNameKey.MaxLength = 250
        Me.txtNameKey.Name = "txtNameKey"
        Me.txtNameKey.Size = New System.Drawing.Size(214, 23)
        Me.txtNameKey.TabIndex = 1
        Me.txtNameKey.Tag = ""
        Me.txtNameKey.Text = " "
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.lblName.ForeColor = System.Drawing.Color.Black
        Me.lblName.Location = New System.Drawing.Point(9, 33)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(60, 15)
        Me.lblName.TabIndex = 21
        Me.lblName.Text = "Name Key"
        '
        'txtCmpName
        '
        Me.txtCmpName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCmpName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCmpName.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.txtCmpName.Location = New System.Drawing.Point(114, 6)
        Me.txtCmpName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtCmpName.MaxLength = 15
        Me.txtCmpName.Name = "txtCmpName"
        Me.txtCmpName.Size = New System.Drawing.Size(355, 23)
        Me.txtCmpName.TabIndex = 0
        '
        'txtCmpAddr
        '
        Me.txtCmpAddr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCmpAddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCmpAddr.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.txtCmpAddr.Location = New System.Drawing.Point(115, 71)
        Me.txtCmpAddr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtCmpAddr.MaxLength = 250
        Me.txtCmpAddr.Name = "txtCmpAddr"
        Me.txtCmpAddr.Size = New System.Drawing.Size(354, 23)
        Me.txtCmpAddr.TabIndex = 2
        '
        'txtAddID
        '
        Me.txtAddID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtAddID.Location = New System.Drawing.Point(602, 13)
        Me.txtAddID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAddID.Name = "txtAddID"
        Me.txtAddID.Size = New System.Drawing.Size(25, 25)
        Me.txtAddID.TabIndex = 38
        Me.txtAddID.Visible = False
        '
        'txtCustID
        '
        Me.txtCustID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtCustID.Location = New System.Drawing.Point(602, 13)
        Me.txtCustID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtCustID.Name = "txtCustID"
        Me.txtCustID.Size = New System.Drawing.Size(25, 25)
        Me.txtCustID.TabIndex = 24
        Me.txtCustID.Visible = False
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(10, 73)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(105, 15)
        Me.Label1.TabIndex = 77
        Me.Label1.Text = "Company Address"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(8, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 15)
        Me.Label5.TabIndex = 81
        Me.Label5.Text = "Company Name"
        '
        'btnedit
        '
        CBlendItems1.iColor = New System.Drawing.Color() {System.Drawing.Color.White, System.Drawing.Color.Blue, System.Drawing.Color.Blue}
        CBlendItems1.iPoint = New Single() {0!, 0.3229814!, 1.0!}
        Me.btnedit.ColorFillBlend = CBlendItems1
        Me.btnedit.DesignerSelected = False
        Me.btnedit.ImageIndex = 0
        Me.btnedit.Location = New System.Drawing.Point(71, 3)
        Me.btnedit.Name = "btnedit"
        Me.btnedit.Size = New System.Drawing.Size(59, 25)
        Me.btnedit.TabIndex = 12
        Me.btnedit.Text = "Cl&ose"
        Me.tt.SetToolTip(Me.btnedit, "Close : Alt+L")
        '
        'btnadd
        '
        Me.btnadd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        CBlendItems2.iColor = New System.Drawing.Color() {System.Drawing.Color.White, System.Drawing.Color.White, System.Drawing.Color.ForestGreen}
        CBlendItems2.iPoint = New Single() {0!, 0.1273292!, 1.0!}
        Me.btnadd.ColorFillBlend = CBlendItems2
        Me.btnadd.ColorFillSolid = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnadd.DesignerSelected = False
        Me.btnadd.ForeColor = System.Drawing.Color.Black
        Me.btnadd.ImageIndex = 0
        Me.btnadd.Location = New System.Drawing.Point(6, 3)
        Me.btnadd.Name = "btnadd"
        Me.btnadd.Size = New System.Drawing.Size(59, 25)
        Me.btnadd.TabIndex = 40
        Me.btnadd.Text = "&Save"
        Me.tt.SetToolTip(Me.btnadd, "Save : Alt+S")
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCancelKey)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSaveKey)
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(304, 157)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(1)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(165, 32)
        Me.FlowLayoutPanel1.TabIndex = 91
        '
        'btnKB
        '
        Me.btnKB.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnKB.BackColor = System.Drawing.Color.White
        Me.btnKB.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnKB.Image = CType(resources.GetObject("btnKB.Image"), System.Drawing.Image)
        Me.btnKB.Location = New System.Drawing.Point(565, -135)
        Me.btnKB.Name = "btnKB"
        Me.btnKB.Size = New System.Drawing.Size(42, 20)
        Me.btnKB.TabIndex = 92
        Me.btnKB.UseVisualStyleBackColor = False
        '
        'txtAddrKey
        '
        Me.txtAddrKey.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAddrKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAddrKey.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.txtAddrKey.Location = New System.Drawing.Point(115, 93)
        Me.txtAddrKey.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAddrKey.MaxLength = 250
        Me.txtAddrKey.Name = "txtAddrKey"
        Me.txtAddrKey.Size = New System.Drawing.Size(213, 23)
        Me.txtAddrKey.TabIndex = 93
        Me.txtAddrKey.Tag = ""
        Me.txtAddrKey.Text = " "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(10, 97)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 15)
        Me.Label2.TabIndex = 94
        Me.Label2.Text = "Address Key"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 221)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(496, 17)
        Me.Label3.TabIndex = 96
        Me.Label3.Text = "For any Assistance please call Softvent Representative @ 080-2679 4645 / 26793693" &
    ""
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.lblName)
        Me.Panel1.Controls.Add(Me.FlowLayoutPanel1)
        Me.Panel1.Controls.Add(Me.btnSend)
        Me.Panel1.Controls.Add(Me.txtNameKey)
        Me.Panel1.Controls.Add(Me.txtAddrKey)
        Me.Panel1.Controls.Add(Me.txtCmpAddr)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.txtCmpName)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(490, 192)
        Me.Panel1.TabIndex = 97
        '
        'btnSaveKey
        '
        Me.btnSaveKey.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSaveKey.BackgroundImage = Global.vICAxon.My.Resources.Resources.T3_SAVE
        Me.btnSaveKey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSaveKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSaveKey.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveKey.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSaveKey.Location = New System.Drawing.Point(6, 5)
        Me.btnSaveKey.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSaveKey.Name = "btnSaveKey"
        Me.btnSaveKey.Size = New System.Drawing.Size(72, 26)
        Me.btnSaveKey.TabIndex = 97
        Me.btnSaveKey.Text = "&Save"
        Me.btnSaveKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSaveKey.UseVisualStyleBackColor = False
        '
        'btnCancelKey
        '
        Me.btnCancelKey.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelKey.BackgroundImage = Global.vICAxon.My.Resources.Resources.T3_CANCEL
        Me.btnCancelKey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancelKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancelKey.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancelKey.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancelKey.Location = New System.Drawing.Point(86, 4)
        Me.btnCancelKey.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancelKey.Name = "btnCancelKey"
        Me.btnCancelKey.Size = New System.Drawing.Size(75, 27)
        Me.btnCancelKey.TabIndex = 96
        Me.btnCancelKey.Text = "&Cancel"
        Me.btnCancelKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancelKey.UseVisualStyleBackColor = False
        '
        'btnSend
        '
        Me.btnSend.BorderColor = System.Drawing.Color.Black
        CBlendItems3.iColor = New System.Drawing.Color() {System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.White}
        CBlendItems3.iPoint = New Single() {0!, 1.0!}
        Me.btnSend.ColorFillBlend = CBlendItems3
        Me.btnSend.ColorFillSolid = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnSend.DesignerSelected = False
        Me.btnSend.ForeColor = System.Drawing.Color.Black
        Me.btnSend.ImageIndex = 0
        Me.btnSend.Location = New System.Drawing.Point(13, 157)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(164, 25)
        Me.btnSend.TabIndex = 95
        Me.btnSend.Text = "Send Mail to Softvent"
        '
        'frmUtlCompKey
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(516, 253)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnKB)
        Me.Controls.Add(Me.txtCustID)
        Me.Controls.Add(Me.txtAddID)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(522, 281)
        Me.Name = "frmUtlCompKey"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Company Key..."
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtNameKey As System.Windows.Forms.TextBox
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtCmpAddr As System.Windows.Forms.TextBox
    Friend WithEvents txtCmpName As System.Windows.Forms.TextBox
    Friend WithEvents txtCustID As System.Windows.Forms.TextBox
    Friend WithEvents txtAddID As System.Windows.Forms.TextBox
    Friend WithEvents btnadd As vICAxon.CButton
    Friend WithEvents btnedit As vICAxon.CButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tt As System.Windows.Forms.ToolTip
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnKB As System.Windows.Forms.Button
    Friend WithEvents txtAddrKey As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSend As vICAxon.CButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCancelKey As C1.Win.C1Input.C1Button
    Friend WithEvents btnSaveKey As C1.Win.C1Input.C1Button
End Class
