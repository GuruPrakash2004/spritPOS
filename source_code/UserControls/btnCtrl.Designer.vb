<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class btnCtrl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(btnCtrl))
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnMoveFirst = New System.Windows.Forms.Button()
        Me.btnMovePrev = New System.Windows.Forms.Button()
        Me.btnMoveNext = New System.Windows.Forms.Button()
        Me.btnMoveLast = New System.Windows.Forms.Button()
        Me.btnOptions = New C1.Win.C1Input.C1SplitButton()
        Me.btnFind = New System.Windows.Forms.Button()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.mnuSettings = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.mnuSettings.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnNew
        '
        Me.btnNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNew.AutoSize = True
        Me.btnNew.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNew.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnNew.ForeColor = System.Drawing.Color.Black
        Me.btnNew.Image = CType(resources.GetObject("btnNew.Image"), System.Drawing.Image)
        Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNew.Location = New System.Drawing.Point(6, 0)
        Me.btnNew.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 30)
        Me.btnNew.TabIndex = 0
        Me.btnNew.Tag = "New"
        Me.btnNew.Text = "&New"
        Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnNew, "Alt + N")
        Me.btnNew.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.AutoSize = True
        Me.btnEdit.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnEdit.ForeColor = System.Drawing.Color.Black
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(98, 0)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(90, 30)
        Me.btnEdit.TabIndex = 1
        Me.btnEdit.Tag = "Edit"
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnEdit, "Alt + E")
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.AutoSize = True
        Me.btnDelete.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnDelete.ForeColor = System.Drawing.Color.Black
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(190, 0)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(90, 30)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Tag = "Delete"
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnDelete, "ALt + D")
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.AutoSize = True
        Me.btnPrint.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnPrint.ForeColor = System.Drawing.Color.Black
        Me.btnPrint.Image = CType(resources.GetObject("btnPrint.Image"), System.Drawing.Image)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(374, 0)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(90, 30)
        Me.btnPrint.TabIndex = 4
        Me.btnPrint.Tag = "Print"
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnPrint, "ALt + P")
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.AutoSize = True
        Me.btnClose.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ForeColor = System.Drawing.Color.Black
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(282, 0)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Tag = "Close"
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnClose, "ALt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnMoveFirst
        '
        Me.btnMoveFirst.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMoveFirst.AutoSize = True
        Me.btnMoveFirst.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnMoveFirst.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnMoveFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMoveFirst.ForeColor = System.Drawing.Color.Black
        Me.btnMoveFirst.Image = CType(resources.GetObject("btnMoveFirst.Image"), System.Drawing.Image)
        Me.btnMoveFirst.Location = New System.Drawing.Point(588, 0)
        Me.btnMoveFirst.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnMoveFirst.Name = "btnMoveFirst"
        Me.btnMoveFirst.Size = New System.Drawing.Size(30, 30)
        Me.btnMoveFirst.TabIndex = 7
        Me.btnMoveFirst.Tag = "MoveFirst"
        Me.btnMoveFirst.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnMoveFirst, "First")
        Me.btnMoveFirst.UseVisualStyleBackColor = False
        '
        'btnMovePrev
        '
        Me.btnMovePrev.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMovePrev.AutoSize = True
        Me.btnMovePrev.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnMovePrev.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnMovePrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMovePrev.ForeColor = System.Drawing.Color.Black
        Me.btnMovePrev.Image = CType(resources.GetObject("btnMovePrev.Image"), System.Drawing.Image)
        Me.btnMovePrev.Location = New System.Drawing.Point(620, 0)
        Me.btnMovePrev.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnMovePrev.Name = "btnMovePrev"
        Me.btnMovePrev.Size = New System.Drawing.Size(30, 30)
        Me.btnMovePrev.TabIndex = 6
        Me.btnMovePrev.Tag = "MovePrev"
        Me.tt.SetToolTip(Me.btnMovePrev, "Previous")
        Me.btnMovePrev.UseVisualStyleBackColor = False
        '
        'btnMoveNext
        '
        Me.btnMoveNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMoveNext.AutoSize = True
        Me.btnMoveNext.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnMoveNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnMoveNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMoveNext.ForeColor = System.Drawing.Color.Black
        Me.btnMoveNext.Image = CType(resources.GetObject("btnMoveNext.Image"), System.Drawing.Image)
        Me.btnMoveNext.Location = New System.Drawing.Point(684, 0)
        Me.btnMoveNext.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnMoveNext.Name = "btnMoveNext"
        Me.btnMoveNext.Size = New System.Drawing.Size(30, 30)
        Me.btnMoveNext.TabIndex = 9
        Me.btnMoveNext.Tag = "MoveNext"
        Me.btnMoveNext.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnMoveNext, "Next")
        Me.btnMoveNext.UseVisualStyleBackColor = False
        '
        'btnMoveLast
        '
        Me.btnMoveLast.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMoveLast.AutoSize = True
        Me.btnMoveLast.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnMoveLast.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnMoveLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMoveLast.ForeColor = System.Drawing.Color.Black
        Me.btnMoveLast.Image = CType(resources.GetObject("btnMoveLast.Image"), System.Drawing.Image)
        Me.btnMoveLast.Location = New System.Drawing.Point(716, 0)
        Me.btnMoveLast.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnMoveLast.Name = "btnMoveLast"
        Me.btnMoveLast.Size = New System.Drawing.Size(30, 30)
        Me.btnMoveLast.TabIndex = 10
        Me.btnMoveLast.Tag = "MoveLast"
        Me.btnMoveLast.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnMoveLast, "Last")
        Me.btnMoveLast.UseVisualStyleBackColor = False
        '
        'btnOptions
        '
        Me.btnOptions.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOptions.AutoSize = True
        Me.btnOptions.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOptions.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnOptions.ForeColor = System.Drawing.Color.Black
        Me.btnOptions.Image = CType(resources.GetObject("btnOptions.Image"), System.Drawing.Image)
        Me.btnOptions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOptions.Location = New System.Drawing.Point(466, 0)
        Me.btnOptions.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnOptions.Name = "btnOptions"
        Me.btnOptions.Size = New System.Drawing.Size(120, 30)
        Me.btnOptions.TabIndex = 5
        Me.btnOptions.Tag = "Options"
        Me.btnOptions.Text = "Options"
        Me.btnOptions.UseVisualStyleBackColor = False
        '
        'btnFind
        '
        Me.btnFind.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFind.AutoSize = True
        Me.btnFind.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFind.ForeColor = System.Drawing.Color.Black
        Me.btnFind.Image = CType(resources.GetObject("btnFind.Image"), System.Drawing.Image)
        Me.btnFind.Location = New System.Drawing.Point(652, 0)
        Me.btnFind.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(30, 30)
        Me.btnFind.TabIndex = 8
        Me.btnFind.Tag = "Find"
        Me.btnFind.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnFind, "Search")
        Me.btnFind.UseVisualStyleBackColor = False
        '
        'tt
        '
        Me.tt.ForeColor = System.Drawing.Color.Black
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.AutoSize = True
        Me.btnCancel.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCancel.CausesValidation = False
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(840, 0)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Tag = "Cancel"
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnCancel, "Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.AutoSize = True
        Me.btnSave.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSave.ForeColor = System.Drawing.Color.Black
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(748, 0)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 11
        Me.btnSave.Tag = "Save"
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnSave, "Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'mnuSettings
        '
        Me.mnuSettings.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingsToolStripMenuItem})
        Me.mnuSettings.Name = "mnuSettings"
        Me.mnuSettings.Size = New System.Drawing.Size(126, 26)
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.SettingsToolStripMenuItem.Text = "Settings..."
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCancel)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSave)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnMoveLast)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnMoveNext)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFind)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnMovePrev)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnMoveFirst)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnOptions)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnPrint)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnClose)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnDelete)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnEdit)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnNew)
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(7, 2)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(932, 34)
        Me.FlowLayoutPanel1.TabIndex = 0
        '
        'btnCtrl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.CausesValidation = False
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "btnCtrl"
        Me.Size = New System.Drawing.Size(944, 36)
        Me.mnuSettings.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnMoveFirst As System.Windows.Forms.Button
    Friend WithEvents btnMovePrev As System.Windows.Forms.Button
    Friend WithEvents btnMoveNext As System.Windows.Forms.Button
    Friend WithEvents btnMoveLast As System.Windows.Forms.Button
    Friend WithEvents btnOptions As C1.Win.C1Input.C1SplitButton
    Friend WithEvents btnFind As System.Windows.Forms.Button
    Friend WithEvents tt As System.Windows.Forms.ToolTip
    Friend WithEvents mnuSettings As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Public WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
