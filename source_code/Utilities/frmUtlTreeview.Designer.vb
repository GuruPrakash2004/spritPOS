<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlTreeview
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlTreeview))
        Me.tvBOM = New System.Windows.Forms.TreeView()
        Me.btnExpand = New C1.Win.C1Input.C1Button()
        Me.btnPring = New C1.Win.C1Input.C1Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtFind = New System.Windows.Forms.TextBox()
        Me.btnClearSerach = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnCollapse = New C1.Win.C1Input.C1Button()
        Me.txtBOMID = New System.Windows.Forms.TextBox()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tvBOM
        '
        Me.tvBOM.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvBOM.Location = New System.Drawing.Point(1, 12)
        Me.tvBOM.Name = "tvBOM"
        Me.tvBOM.Size = New System.Drawing.Size(850, 340)
        Me.tvBOM.TabIndex = 116
        '
        'btnExpand
        '
        Me.btnExpand.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExpand.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExpand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExpand.Location = New System.Drawing.Point(7, 6)
        Me.btnExpand.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExpand.Name = "btnExpand"
        Me.btnExpand.Size = New System.Drawing.Size(80, 29)
        Me.btnExpand.TabIndex = 117
        Me.btnExpand.Text = "Expand All"
        Me.btnExpand.UseVisualStyleBackColor = False
        '
        'btnPring
        '
        Me.btnPring.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPring.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnPring.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPring.Location = New System.Drawing.Point(158, 6)
        Me.btnPring.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPring.Name = "btnPring"
        Me.btnPring.Size = New System.Drawing.Size(65, 29)
        Me.btnPring.TabIndex = 118
        Me.btnPring.Text = "Print"
        Me.btnPring.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(229, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(203, 17)
        Me.Label1.TabIndex = 120
        Me.Label1.Text = "To find,Type Text and Press Enter"
        '
        'txtFind
        '
        Me.txtFind.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFind.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtFind.Location = New System.Drawing.Point(437, 8)
        Me.txtFind.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFind.Name = "txtFind"
        Me.txtFind.Size = New System.Drawing.Size(246, 25)
        Me.txtFind.TabIndex = 119
        '
        'btnClearSerach
        '
        Me.btnClearSerach.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearSerach.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClearSerach.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClearSerach.Location = New System.Drawing.Point(685, 6)
        Me.btnClearSerach.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClearSerach.Name = "btnClearSerach"
        Me.btnClearSerach.Size = New System.Drawing.Size(75, 29)
        Me.btnClearSerach.TabIndex = 121
        Me.btnClearSerach.Text = "Clear"
        Me.btnClearSerach.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(759, 6)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 29)
        Me.btnClose.TabIndex = 122
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.btnCollapse)
        Me.Panel1.Controls.Add(Me.btnExpand)
        Me.Panel1.Controls.Add(Me.btnClose)
        Me.Panel1.Controls.Add(Me.btnClearSerach)
        Me.Panel1.Controls.Add(Me.btnPring)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtFind)
        Me.Panel1.Location = New System.Drawing.Point(3, 360)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(839, 39)
        Me.Panel1.TabIndex = 123
        '
        'btnCollapse
        '
        Me.btnCollapse.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCollapse.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCollapse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCollapse.Location = New System.Drawing.Point(88, 6)
        Me.btnCollapse.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCollapse.Name = "btnCollapse"
        Me.btnCollapse.Size = New System.Drawing.Size(69, 28)
        Me.btnCollapse.TabIndex = 123
        Me.btnCollapse.Text = "Collapse"
        Me.btnCollapse.UseVisualStyleBackColor = False
        '
        'txtBOMID
        '
        Me.txtBOMID.Location = New System.Drawing.Point(39, 0)
        Me.txtBOMID.Name = "txtBOMID"
        Me.txtBOMID.Size = New System.Drawing.Size(100, 20)
        Me.txtBOMID.TabIndex = 124
        Me.txtBOMID.Visible = False
        '
        'frmUtlTreeview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(854, 403)
        Me.Controls.Add(Me.txtBOMID)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.tvBOM)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmUtlTreeview"
        Me.Text = "Tree View"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents tvBOM As System.Windows.Forms.TreeView
    Friend WithEvents btnExpand As C1.Win.C1Input.C1Button
    Friend WithEvents btnPring As C1.Win.C1Input.C1Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFind As System.Windows.Forms.TextBox
    Friend WithEvents btnClearSerach As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCollapse As C1.Win.C1Input.C1Button
    Friend WithEvents txtBOMID As System.Windows.Forms.TextBox
End Class
