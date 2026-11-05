<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlAssignRight
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlAssignRight))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtUser = New System.Windows.Forms.TextBox()
        Me.btnSelUser = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "User"
        '
        'txtUser
        '
        Me.txtUser.Enabled = False
        Me.txtUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUser.Location = New System.Drawing.Point(71, 6)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(166, 22)
        Me.txtUser.TabIndex = 1
        '
        'btnSelUser
        '
        Me.btnSelUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelUser.Location = New System.Drawing.Point(252, 6)
        Me.btnSelUser.Name = "btnSelUser"
        Me.btnSelUser.Size = New System.Drawing.Size(26, 23)
        Me.btnSelUser.TabIndex = 2
        Me.btnSelUser.Text = "..."
        Me.btnSelUser.UseVisualStyleBackColor = True
        '
        'vsfDet
        '
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.ColumnInfo = "5,0,0,0,0,120,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.vsfDet.Location = New System.Drawing.Point(11, 50)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(2)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 1
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Size = New System.Drawing.Size(652, 362)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 25
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(571, 417)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(92, 28)
        Me.btnClose.TabIndex = 223
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(473, 417)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(92, 28)
        Me.btnSave.TabIndex = 222
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'chkAll
        '
        Me.chkAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkAll.AutoSize = True
        Me.chkAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAll.Location = New System.Drawing.Point(11, 421)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(94, 24)
        Me.chkAll.TabIndex = 224
        Me.chkAll.Text = "Select &All"
        Me.chkAll.UseVisualStyleBackColor = True
        '
        'txtDummy
        '
        Me.txtDummy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDummy.Location = New System.Drawing.Point(312, 9)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(33, 22)
        Me.txtDummy.TabIndex = 225
        Me.txtDummy.Visible = False
        '
        'frmUtlAssignRight
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(674, 457)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.chkAll)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnSelUser)
        Me.Controls.Add(Me.txtUser)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmUtlAssignRight"
        Me.ShowIcon = False
        Me.Tag = "RptRights"
        Me.Text = "Assign Rights"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents txtUser As TextBox
    Friend WithEvents btnSelUser As Button
    Private WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents chkAll As CheckBox
    Friend WithEvents txtDummy As TextBox
End Class
