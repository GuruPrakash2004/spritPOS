<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlAddItems
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlAddItems))
        Me.pnlImport = New System.Windows.Forms.Panel()
        Me.Delete = New System.Windows.Forms.Button()
        Me.btnLoadItems = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.vsfItems = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnItemValidation = New System.Windows.Forms.Button()
        Me.txtData = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.pnlImport.SuspendLayout()
        CType(Me.vsfItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlImport
        '
        Me.pnlImport.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.pnlImport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlImport.Controls.Add(Me.Delete)
        Me.pnlImport.Controls.Add(Me.btnLoadItems)
        Me.pnlImport.Controls.Add(Me.Label5)
        Me.pnlImport.Controls.Add(Me.Label6)
        Me.pnlImport.Controls.Add(Me.vsfItems)
        Me.pnlImport.Controls.Add(Me.btnItemValidation)
        Me.pnlImport.Controls.Add(Me.txtData)
        Me.pnlImport.Location = New System.Drawing.Point(1, 2)
        Me.pnlImport.Name = "pnlImport"
        Me.pnlImport.Size = New System.Drawing.Size(738, 413)
        Me.pnlImport.TabIndex = 232
        '
        'Delete
        '
        Me.Delete.BackColor = System.Drawing.Color.Transparent
        Me.Delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Delete.Location = New System.Drawing.Point(409, 378)
        Me.Delete.Name = "Delete"
        Me.Delete.Size = New System.Drawing.Size(152, 26)
        Me.Delete.TabIndex = 231
        Me.Delete.Text = "Delete All"
        Me.Delete.UseVisualStyleBackColor = False
        '
        'btnLoadItems
        '
        Me.btnLoadItems.BackColor = System.Drawing.Color.Transparent
        Me.btnLoadItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoadItems.Location = New System.Drawing.Point(576, 378)
        Me.btnLoadItems.Name = "btnLoadItems"
        Me.btnLoadItems.Size = New System.Drawing.Size(152, 26)
        Me.btnLoadItems.TabIndex = 230
        Me.btnLoadItems.Text = "Load Items"
        Me.btnLoadItems.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(11, 380)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(174, 13)
        Me.Label5.TabIndex = 228
        Me.Label5.Text = "After pasting click on >> button"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(11, 5)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(208, 13)
        Me.Label6.TabIndex = 227
        Me.Label6.Text = "Paste the List of Items in the box below"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'vsfItems
        '
        Me.vsfItems.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfItems.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfItems.BackColor = System.Drawing.SystemColors.ControlDark
        Me.vsfItems.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfItems.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfItems.ExtendLastCol = True
        Me.vsfItems.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfItems.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfItems.Location = New System.Drawing.Point(379, 26)
        Me.vsfItems.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfItems.Name = "vsfItems"
        Me.vsfItems.Rows.DefaultSize = 24
        Me.vsfItems.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfItems.Size = New System.Drawing.Size(349, 345)
        Me.vsfItems.StyleInfo = resources.GetString("vsfItems.StyleInfo")
        Me.vsfItems.TabIndex = 226
        Me.vsfItems.Tag = ""
        '
        'btnItemValidation
        '
        Me.btnItemValidation.BackColor = System.Drawing.Color.Transparent
        Me.btnItemValidation.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnItemValidation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnItemValidation.Location = New System.Drawing.Point(318, 182)
        Me.btnItemValidation.Name = "btnItemValidation"
        Me.btnItemValidation.Size = New System.Drawing.Size(54, 26)
        Me.btnItemValidation.TabIndex = 1
        Me.btnItemValidation.Text = ">>>>"
        Me.btnItemValidation.UseVisualStyleBackColor = False
        '
        'txtData
        '
        Me.txtData.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.txtData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtData.Location = New System.Drawing.Point(11, 26)
        Me.txtData.Multiline = True
        Me.txtData.Name = "txtData"
        Me.txtData.Size = New System.Drawing.Size(296, 345)
        Me.txtData.TabIndex = 0
        '
        'Timer1
        '
        '
        'Timer2
        '
        '
        'frmUtlAddItems
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(731, 417)
        Me.Controls.Add(Me.pnlImport)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(747, 456)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(747, 445)
        Me.Name = "frmUtlAddItems"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Items"
        Me.pnlImport.ResumeLayout(False)
        Me.pnlImport.PerformLayout()
        CType(Me.vsfItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlImport As System.Windows.Forms.Panel
    Friend WithEvents btnLoadItems As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents vsfItems As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnItemValidation As System.Windows.Forms.Button
    Friend WithEvents txtData As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents Delete As Button
End Class
