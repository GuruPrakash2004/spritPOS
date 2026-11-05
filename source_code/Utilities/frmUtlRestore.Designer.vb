<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlRestore
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlRestore))
        Me.vsfAS = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnDel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkRemove = New System.Windows.Forms.CheckBox()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        CType(Me.vsfAS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfAS
        '
        Me.vsfAS.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfAS.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfAS.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfAS.ExtendLastCol = True
        Me.vsfAS.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.vsfAS.Location = New System.Drawing.Point(3, 0)
        Me.vsfAS.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.vsfAS.Name = "vsfAS"
        Me.vsfAS.Rows.DefaultSize = 24
        Me.vsfAS.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfAS.Size = New System.Drawing.Size(744, 314)
        Me.vsfAS.TabIndex = 98
        '
        'btnDel
        '
        Me.btnDel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDel.Location = New System.Drawing.Point(205, 363)
        Me.btnDel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnDel.Name = "btnDel"
        Me.btnDel.Size = New System.Drawing.Size(124, 33)
        Me.btnDel.TabIndex = 99
        Me.btnDel.Text = "Remove"
        Me.btnDel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(365, 332)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(395, 63)
        Me.Label1.TabIndex = 100
        Me.Label1.Text = "Double Click a file from the above list and restore the data."
        '
        'chkRemove
        '
        Me.chkRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkRemove.AutoSize = True
        Me.chkRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRemove.Location = New System.Drawing.Point(16, 326)
        Me.chkRemove.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkRemove.Name = "chkRemove"
        Me.chkRemove.Size = New System.Drawing.Size(106, 29)
        Me.chkRemove.TabIndex = 101
        Me.chkRemove.Text = "Remove"
        Me.chkRemove.UseVisualStyleBackColor = True
        '
        'chkSelectAll
        '
        Me.chkSelectAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelectAll.Location = New System.Drawing.Point(16, 367)
        Me.chkSelectAll.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(116, 29)
        Me.chkSelectAll.TabIndex = 102
        Me.chkSelectAll.Text = "Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        Me.chkSelectAll.Visible = False
        '
        'frmUtlRestore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(752, 406)
        Me.Controls.Add(Me.chkSelectAll)
        Me.Controls.Add(Me.chkRemove)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnDel)
        Me.Controls.Add(Me.vsfAS)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(599, 399)
        Me.Name = "frmUtlRestore"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Select Restore Values"
        CType(Me.vsfAS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents vsfAS As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnDel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkRemove As CheckBox
    Friend WithEvents chkSelectAll As CheckBox
End Class
