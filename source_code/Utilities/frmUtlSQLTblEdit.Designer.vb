<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlSQLTblEdit
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlSQLTblEdit))
        Me.dv = New System.Windows.Forms.DataGridView()
        Me.txtEdit = New System.Windows.Forms.TextBox()
        Me.SC1 = New System.Windows.Forms.SplitContainer()
        Me.lblTblName = New System.Windows.Forms.Label()
        Me.lblRecords = New System.Windows.Forms.Label()
        Me.btnReload = New System.Windows.Forms.Button()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        CType(Me.dv, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SC1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SC1.Panel1.SuspendLayout()
        Me.SC1.Panel2.SuspendLayout()
        Me.SC1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dv
        '
        Me.dv.AllowUserToOrderColumns = True
        Me.dv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dv.Location = New System.Drawing.Point(3, 3)
        Me.dv.Name = "dv"
        Me.dv.Size = New System.Drawing.Size(708, 209)
        Me.dv.TabIndex = 230
        '
        'txtEdit
        '
        Me.txtEdit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEdit.Location = New System.Drawing.Point(3, 3)
        Me.txtEdit.Multiline = True
        Me.txtEdit.Name = "txtEdit"
        Me.txtEdit.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtEdit.Size = New System.Drawing.Size(708, 130)
        Me.txtEdit.TabIndex = 235
        '
        'SC1
        '
        Me.SC1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SC1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SC1.Location = New System.Drawing.Point(12, 40)
        Me.SC1.Name = "SC1"
        Me.SC1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SC1.Panel1
        '
        Me.SC1.Panel1.Controls.Add(Me.txtEdit)
        '
        'SC1.Panel2
        '
        Me.SC1.Panel2.Controls.Add(Me.dv)
        Me.SC1.Size = New System.Drawing.Size(716, 359)
        Me.SC1.SplitterDistance = 138
        Me.SC1.TabIndex = 236
        '
        'lblTblName
        '
        Me.lblTblName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTblName.AutoSize = True
        Me.lblTblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTblName.ForeColor = System.Drawing.Color.Blue
        Me.lblTblName.Location = New System.Drawing.Point(3, 0)
        Me.lblTblName.Name = "lblTblName"
        Me.lblTblName.Size = New System.Drawing.Size(45, 15)
        Me.lblTblName.TabIndex = 238
        Me.lblTblName.Text = "Master"
        Me.lblTblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblRecords
        '
        Me.lblRecords.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRecords.AutoSize = True
        Me.lblRecords.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecords.ForeColor = System.Drawing.Color.Blue
        Me.lblRecords.Location = New System.Drawing.Point(54, 0)
        Me.lblRecords.Name = "lblRecords"
        Me.lblRecords.Size = New System.Drawing.Size(95, 15)
        Me.lblRecords.TabIndex = 239
        Me.lblRecords.Text = "Record Number"
        Me.lblRecords.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnReload
        '
        Me.btnReload.BackgroundImage = CType(resources.GetObject("btnReload.BackgroundImage"), System.Drawing.Image)
        Me.btnReload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnReload.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReload.Location = New System.Drawing.Point(12, 6)
        Me.btnReload.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnReload.Name = "btnReload"
        Me.btnReload.Size = New System.Drawing.Size(77, 30)
        Me.btnReload.TabIndex = 237
        Me.btnReload.Text = "Reload"
        Me.btnReload.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReload.UseVisualStyleBackColor = False
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(530, 406)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(33, 31)
        Me.btnExport.TabIndex = 234
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(651, 406)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(76, 31)
        Me.btnClose.TabIndex = 233
        Me.btnClose.Text = "C&lose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Location = New System.Drawing.Point(569, 405)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 31)
        Me.btnSave.TabIndex = 231
        Me.btnSave.Text = "Update"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblTblName)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblRecords)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(12, 412)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(380, 19)
        Me.FlowLayoutPanel1.TabIndex = 241
        '
        'frmUtlSQLTblEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(740, 450)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.btnReload)
        Me.Controls.Add(Me.SC1)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSave)
        Me.Name = "frmUtlSQLTblEdit"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        CType(Me.dv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SC1.Panel1.ResumeLayout(False)
        Me.SC1.Panel1.PerformLayout()
        Me.SC1.Panel2.ResumeLayout(False)
        CType(Me.SC1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SC1.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dv As DataGridView
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents txtEdit As TextBox
    Friend WithEvents SC1 As SplitContainer
    Friend WithEvents btnReload As Button
    Friend WithEvents lblTblName As Label
    Friend WithEvents lblRecords As Label
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
End Class
