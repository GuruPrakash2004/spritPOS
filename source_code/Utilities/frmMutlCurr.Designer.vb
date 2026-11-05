<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMutlCurr
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMutlCurr))
        Me.txtTotAMT = New System.Windows.Forms.TextBox()
        Me.vsfcur = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        CType(Me.vsfcur, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtTotAMT
        '
        Me.txtTotAMT.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTotAMT.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotAMT.Location = New System.Drawing.Point(7, 438)
        Me.txtTotAMT.Name = "txtTotAMT"
        Me.txtTotAMT.ReadOnly = True
        Me.txtTotAMT.Size = New System.Drawing.Size(152, 29)
        Me.txtTotAMT.TabIndex = 16
        '
        'vsfcur
        '
        Me.vsfcur.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.Both
        Me.vsfcur.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfcur.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfcur.ColumnInfo = resources.GetString("vsfcur.ColumnInfo")
        Me.vsfcur.ExtendLastCol = True
        Me.vsfcur.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold)
        Me.vsfcur.Location = New System.Drawing.Point(6, 9)
        Me.vsfcur.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfcur.Name = "vsfcur"
        Me.vsfcur.Rows.DefaultSize = 23
        Me.vsfcur.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfcur.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell
        Me.vsfcur.ShowCellLabels = True
        Me.vsfcur.Size = New System.Drawing.Size(370, 422)
        Me.vsfcur.StyleInfo = resources.GetString("vsfcur.StyleInfo")
        Me.vsfcur.TabIndex = 273
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(34, 470)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(308, 13)
        Me.Label1.TabIndex = 275
        Me.Label1.Text = "Tip: Double Click On The Currency Image To Increase The Nos"
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear.BackColor = System.Drawing.Color.Silver
        Me.btnClear.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClear.Location = New System.Drawing.Point(231, 438)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(68, 30)
        Me.btnClear.TabIndex = 276
        Me.btnClear.Text = "Clear"
        Me.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.Silver
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(307, 438)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(77, 30)
        Me.btnCancel.TabIndex = 274
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.BackColor = System.Drawing.Color.White
        Me.btnOK.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_ok
        Me.btnOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(165, 438)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(57, 30)
        Me.btnOK.TabIndex = 17
        Me.btnOK.Text = "&OK"
        Me.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'frmMutlCurr
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ClientSize = New System.Drawing.Size(386, 493)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.vsfcur)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.txtTotAMT)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMutlCurr"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Denomination"
        CType(Me.vsfcur, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTotAMT As TextBox
    Friend WithEvents btnOK As Button
    Friend WithEvents vsfcur As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnCancel As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents btnClear As Button
End Class
