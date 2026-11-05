<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlToolTipText
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
        Me.txtCtrlName = New System.Windows.Forms.TextBox()
        Me.txtFrmName = New System.Windows.Forms.TextBox()
        Me.lblTooltipText = New System.Windows.Forms.Label()
        Me.lblControlName = New System.Windows.Forms.Label()
        Me.lblfrmName = New System.Windows.Forms.Label()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtToolTipText = New System.Windows.Forms.TextBox()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'txtCtrlName
        '
        Me.txtCtrlName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCtrlName.Location = New System.Drawing.Point(162, 97)
        Me.txtCtrlName.Name = "txtCtrlName"
        Me.txtCtrlName.Size = New System.Drawing.Size(343, 26)
        Me.txtCtrlName.TabIndex = 27
        '
        'txtFrmName
        '
        Me.txtFrmName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFrmName.Location = New System.Drawing.Point(162, 56)
        Me.txtFrmName.MaxLength = 50
        Me.txtFrmName.Name = "txtFrmName"
        Me.txtFrmName.Size = New System.Drawing.Size(343, 26)
        Me.txtFrmName.TabIndex = 26
        '
        'lblTooltipText
        '
        Me.lblTooltipText.AutoSize = True
        Me.lblTooltipText.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTooltipText.Location = New System.Drawing.Point(7, 146)
        Me.lblTooltipText.Name = "lblTooltipText"
        Me.lblTooltipText.Size = New System.Drawing.Size(137, 20)
        Me.lblTooltipText.TabIndex = 21
        Me.lblTooltipText.Text = "Enter ToolTip Text"
        '
        'lblControlName
        '
        Me.lblControlName.AutoSize = True
        Me.lblControlName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblControlName.Location = New System.Drawing.Point(7, 100)
        Me.lblControlName.Name = "lblControlName"
        Me.lblControlName.Size = New System.Drawing.Size(149, 20)
        Me.lblControlName.TabIndex = 20
        Me.lblControlName.Text = "Enter Control Name"
        '
        'lblfrmName
        '
        Me.lblfrmName.AutoSize = True
        Me.lblfrmName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfrmName.Location = New System.Drawing.Point(7, 58)
        Me.lblfrmName.Name = "lblfrmName"
        Me.lblfrmName.Size = New System.Drawing.Size(135, 20)
        Me.lblfrmName.TabIndex = 19
        Me.lblfrmName.Text = "Enter Form Name"
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.Location = New System.Drawing.Point(7, 18)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(60, 20)
        Me.lblSearch.TabIndex = 18
        Me.lblSearch.Text = "Search"
        '
        'txtToolTipText
        '
        Me.txtToolTipText.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtToolTipText.Location = New System.Drawing.Point(162, 138)
        Me.txtToolTipText.Multiline = True
        Me.txtToolTipText.Name = "txtToolTipText"
        Me.txtToolTipText.Size = New System.Drawing.Size(343, 62)
        Me.txtToolTipText.TabIndex = 17
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(163, 15)
        Me.txtSearch.MaxLength = 50
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(269, 26)
        Me.txtSearch.TabIndex = 16
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnSave.ForeColor = System.Drawing.Color.Black
        Me.btnSave.Image = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.Location = New System.Drawing.Point(218, 217)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 31)
        Me.btnSave.TabIndex = 28
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.Image = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.Location = New System.Drawing.Point(314, 217)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(92, 31)
        Me.btnCancel.TabIndex = 29
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnClose.ForeColor = System.Drawing.Color.Black
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.Location = New System.Drawing.Point(412, 217)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 31)
        Me.btnClose.TabIndex = 30
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnLoad
        '
        Me.btnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnLoad.ForeColor = System.Drawing.Color.Black
        Me.btnLoad.Location = New System.Drawing.Point(442, 12)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(58, 31)
        Me.btnLoad.TabIndex = 31
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'frmUtlToolTipText
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(522, 260)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtCtrlName)
        Me.Controls.Add(Me.txtFrmName)
        Me.Controls.Add(Me.lblTooltipText)
        Me.Controls.Add(Me.lblControlName)
        Me.Controls.Add(Me.lblfrmName)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.txtToolTipText)
        Me.Controls.Add(Me.txtSearch)
        Me.Name = "frmUtlToolTipText"
        Me.ShowIcon = False
        Me.Text = "Tool Tip Text"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtCtrlName As TextBox
    Friend WithEvents txtFrmName As TextBox
    Friend WithEvents lblTooltipText As Label
    Friend WithEvents lblControlName As Label
    Friend WithEvents lblfrmName As Label
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtToolTipText As TextBox
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents ToolTip1 As ToolTip
End Class
