<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlLicenseReq
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlLicenseReq))
        Me.lblMacID = New System.Windows.Forms.Label()
        Me.txtMacID = New System.Windows.Forms.TextBox()
        Me.txtCompName = New System.Windows.Forms.TextBox()
        Me.lblCompName = New System.Windows.Forms.Label()
        Me.txtAdd1 = New System.Windows.Forms.TextBox()
        Me.lblAdd = New System.Windows.Forms.Label()
        Me.txtAdd2 = New System.Windows.Forms.TextBox()
        Me.txtAdd3 = New System.Windows.Forms.TextBox()
        Me.txtAdd4 = New System.Windows.Forms.TextBox()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtServerName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblMacID
        '
        Me.lblMacID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMacID.AutoSize = True
        Me.lblMacID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblMacID.Location = New System.Drawing.Point(12, 27)
        Me.lblMacID.Name = "lblMacID"
        Me.lblMacID.Size = New System.Drawing.Size(65, 20)
        Me.lblMacID.TabIndex = 0
        Me.lblMacID.Tag = "R"
        Me.lblMacID.Text = "Number"
        '
        'txtMacID
        '
        Me.txtMacID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMacID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtMacID.Location = New System.Drawing.Point(140, 24)
        Me.txtMacID.MaxLength = 50
        Me.txtMacID.Name = "txtMacID"
        Me.txtMacID.Size = New System.Drawing.Size(464, 26)
        Me.txtMacID.TabIndex = 0
        '
        'txtCompName
        '
        Me.txtCompName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCompName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtCompName.Location = New System.Drawing.Point(140, 93)
        Me.txtCompName.MaxLength = 250
        Me.txtCompName.Name = "txtCompName"
        Me.txtCompName.Size = New System.Drawing.Size(464, 26)
        Me.txtCompName.TabIndex = 1
        '
        'lblCompName
        '
        Me.lblCompName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCompName.AutoSize = True
        Me.lblCompName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblCompName.Location = New System.Drawing.Point(12, 97)
        Me.lblCompName.Name = "lblCompName"
        Me.lblCompName.Size = New System.Drawing.Size(122, 20)
        Me.lblCompName.TabIndex = 2
        Me.lblCompName.Tag = "R"
        Me.lblCompName.Text = "Company Name"
        '
        'txtAdd1
        '
        Me.txtAdd1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAdd1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtAdd1.Location = New System.Drawing.Point(140, 125)
        Me.txtAdd1.MaxLength = 150
        Me.txtAdd1.Name = "txtAdd1"
        Me.txtAdd1.Size = New System.Drawing.Size(464, 26)
        Me.txtAdd1.TabIndex = 2
        '
        'lblAdd
        '
        Me.lblAdd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAdd.AutoSize = True
        Me.lblAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblAdd.Location = New System.Drawing.Point(12, 128)
        Me.lblAdd.Name = "lblAdd"
        Me.lblAdd.Size = New System.Drawing.Size(68, 20)
        Me.lblAdd.TabIndex = 4
        Me.lblAdd.Tag = "R"
        Me.lblAdd.Text = "Address"
        '
        'txtAdd2
        '
        Me.txtAdd2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAdd2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtAdd2.Location = New System.Drawing.Point(140, 151)
        Me.txtAdd2.MaxLength = 150
        Me.txtAdd2.Name = "txtAdd2"
        Me.txtAdd2.Size = New System.Drawing.Size(464, 26)
        Me.txtAdd2.TabIndex = 3
        '
        'txtAdd3
        '
        Me.txtAdd3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAdd3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtAdd3.Location = New System.Drawing.Point(140, 177)
        Me.txtAdd3.MaxLength = 150
        Me.txtAdd3.Name = "txtAdd3"
        Me.txtAdd3.Size = New System.Drawing.Size(464, 26)
        Me.txtAdd3.TabIndex = 4
        '
        'txtAdd4
        '
        Me.txtAdd4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAdd4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtAdd4.Location = New System.Drawing.Point(140, 203)
        Me.txtAdd4.MaxLength = 150
        Me.txtAdd4.Name = "txtAdd4"
        Me.txtAdd4.Size = New System.Drawing.Size(464, 26)
        Me.txtAdd4.TabIndex = 5
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(514, 276)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 31)
        Me.btnClose.TabIndex = 9
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + C")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'txtServerName
        '
        Me.txtServerName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtServerName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtServerName.Location = New System.Drawing.Point(140, 63)
        Me.txtServerName.MaxLength = 50
        Me.txtServerName.Name = "txtServerName"
        Me.txtServerName.Size = New System.Drawing.Size(464, 26)
        Me.txtServerName.TabIndex = 21
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label1.Location = New System.Drawing.Point(12, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(101, 20)
        Me.Label1.TabIndex = 22
        Me.Label1.Tag = "R"
        Me.Label1.Text = "Server Name"
        '
        'frmUtlLicenseReq
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(616, 320)
        Me.Controls.Add(Me.txtServerName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtAdd4)
        Me.Controls.Add(Me.txtAdd3)
        Me.Controls.Add(Me.txtAdd2)
        Me.Controls.Add(Me.txtAdd1)
        Me.Controls.Add(Me.lblAdd)
        Me.Controls.Add(Me.txtCompName)
        Me.Controls.Add(Me.lblCompName)
        Me.Controls.Add(Me.txtMacID)
        Me.Controls.Add(Me.lblMacID)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtlLicenseReq"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "License Info"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblMacID As Label
    Friend WithEvents txtMacID As TextBox
    Friend WithEvents txtCompName As TextBox
    Friend WithEvents lblCompName As Label
    Friend WithEvents txtAdd1 As TextBox
    Friend WithEvents lblAdd As Label
    Friend WithEvents txtAdd2 As TextBox
    Friend WithEvents txtAdd3 As TextBox
    Friend WithEvents txtAdd4 As TextBox
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents txtServerName As TextBox
    Friend WithEvents Label1 As Label
End Class
