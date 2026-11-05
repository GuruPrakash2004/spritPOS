<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlMergeItems
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
        Me.btnMergeItm = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMergeItem = New System.Windows.Forms.TextBox()
        Me.btnToItem = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtToItem = New System.Windows.Forms.TextBox()
        Me.txtTItemID = New System.Windows.Forms.TextBox()
        Me.txtMItemId = New System.Windows.Forms.TextBox()
        Me.btnMerge = New System.Windows.Forms.Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.SuspendLayout()
        '
        'btnMergeItm
        '
        Me.btnMergeItm.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnMergeItm.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnMergeItm.Location = New System.Drawing.Point(493, 30)
        Me.btnMergeItm.Name = "btnMergeItm"
        Me.btnMergeItm.Size = New System.Drawing.Size(27, 22)
        Me.btnMergeItm.TabIndex = 292
        Me.btnMergeItm.Text = "..."
        Me.btnMergeItm.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(33, 33)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 16)
        Me.Label1.TabIndex = 291
        Me.Label1.Text = "Merge Item"
        '
        'txtMergeItem
        '
        Me.txtMergeItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtMergeItem.Location = New System.Drawing.Point(115, 30)
        Me.txtMergeItem.Margin = New System.Windows.Forms.Padding(4)
        Me.txtMergeItem.MaxLength = 100
        Me.txtMergeItem.Name = "txtMergeItem"
        Me.txtMergeItem.Size = New System.Drawing.Size(371, 22)
        Me.txtMergeItem.TabIndex = 290
        '
        'btnToItem
        '
        Me.btnToItem.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnToItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnToItem.Location = New System.Drawing.Point(493, 72)
        Me.btnToItem.Name = "btnToItem"
        Me.btnToItem.Size = New System.Drawing.Size(27, 22)
        Me.btnToItem.TabIndex = 295
        Me.btnToItem.Text = "..."
        Me.btnToItem.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label2.Location = New System.Drawing.Point(54, 72)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 16)
        Me.Label2.TabIndex = 294
        Me.Label2.Text = "To Item"
        '
        'txtToItem
        '
        Me.txtToItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtToItem.Location = New System.Drawing.Point(115, 72)
        Me.txtToItem.Margin = New System.Windows.Forms.Padding(4)
        Me.txtToItem.MaxLength = 100
        Me.txtToItem.Name = "txtToItem"
        Me.txtToItem.Size = New System.Drawing.Size(371, 22)
        Me.txtToItem.TabIndex = 293
        '
        'txtTItemID
        '
        Me.txtTItemID.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTItemID.Location = New System.Drawing.Point(546, 71)
        Me.txtTItemID.Margin = New System.Windows.Forms.Padding(4)
        Me.txtTItemID.MaxLength = 50
        Me.txtTItemID.Name = "txtTItemID"
        Me.txtTItemID.Size = New System.Drawing.Size(85, 23)
        Me.txtTItemID.TabIndex = 297
        Me.txtTItemID.Visible = False
        '
        'txtMItemId
        '
        Me.txtMItemId.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMItemId.Location = New System.Drawing.Point(546, 29)
        Me.txtMItemId.Margin = New System.Windows.Forms.Padding(4)
        Me.txtMItemId.MaxLength = 50
        Me.txtMItemId.Name = "txtMItemId"
        Me.txtMItemId.Size = New System.Drawing.Size(85, 23)
        Me.txtMItemId.TabIndex = 296
        Me.txtMItemId.Visible = False
        '
        'btnMerge
        '
        Me.btnMerge.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMerge.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnMerge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMerge.Location = New System.Drawing.Point(461, 126)
        Me.btnMerge.Name = "btnMerge"
        Me.btnMerge.Size = New System.Drawing.Size(98, 29)
        Me.btnMerge.TabIndex = 298
        Me.btnMerge.Text = "&Merge Item"
        Me.btnMerge.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.D_CLOSE
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(577, 126)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(73, 29)
        Me.btnClose.TabIndex = 299
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'frmUtlMergeItems
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(668, 167)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnMerge)
        Me.Controls.Add(Me.txtTItemID)
        Me.Controls.Add(Me.txtMItemId)
        Me.Controls.Add(Me.btnToItem)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtToItem)
        Me.Controls.Add(Me.btnMergeItm)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtMergeItem)
        Me.Name = "frmUtlMergeItems"
        Me.ShowIcon = False
        Me.Text = "Merge Items"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnMergeItm As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtMergeItem As TextBox
    Friend WithEvents btnToItem As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtToItem As TextBox
    Friend WithEvents txtTItemID As TextBox
    Friend WithEvents txtMItemId As TextBox
    Friend WithEvents btnMerge As Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
End Class
