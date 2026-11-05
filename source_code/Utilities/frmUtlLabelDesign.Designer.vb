<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlLabelDesign
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlLabelDesign))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.CboTemplate = New System.Windows.Forms.ComboBox()
        Me.txtLblID = New System.Windows.Forms.TextBox()
        Me.btnload = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnRemoveQuery = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.vsfColumn = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.PnlCopyFrom = New System.Windows.Forms.Panel()
        Me.btnCopyFrom = New System.Windows.Forms.Button()
        Me.txtCopyItem = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtTable = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCondn = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfColumn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlCopyFrom.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(5, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Template"
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "1,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.vsfDet.Location = New System.Drawing.Point(8, 147)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 21
        Me.vsfDet.Rows.DefaultSize = 21
        Me.vsfDet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfDet.Size = New System.Drawing.Size(652, 262)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 7
        '
        'CboTemplate
        '
        Me.CboTemplate.BackColor = System.Drawing.Color.White
        Me.CboTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.CboTemplate.ForeColor = System.Drawing.Color.Black
        Me.CboTemplate.FormattingEnabled = True
        Me.CboTemplate.Location = New System.Drawing.Point(152, 8)
        Me.CboTemplate.MaxLength = 150
        Me.CboTemplate.Name = "CboTemplate"
        Me.CboTemplate.Size = New System.Drawing.Size(346, 28)
        Me.CboTemplate.TabIndex = 0
        '
        'txtLblID
        '
        Me.txtLblID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtLblID.ForeColor = System.Drawing.SystemColors.Control
        Me.txtLblID.Location = New System.Drawing.Point(699, 7)
        Me.txtLblID.Name = "txtLblID"
        Me.txtLblID.Size = New System.Drawing.Size(56, 25)
        Me.txtLblID.TabIndex = 11
        Me.txtLblID.Visible = False
        '
        'btnload
        '
        Me.btnload.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnload.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnload.ForeColor = System.Drawing.Color.Black
        Me.btnload.Location = New System.Drawing.Point(513, 6)
        Me.btnload.Name = "btnload"
        Me.btnload.Size = New System.Drawing.Size(86, 30)
        Me.btnload.TabIndex = 1
        Me.btnload.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.btnload, "Load : Alt + L")
        Me.btnload.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnAdd.ForeColor = System.Drawing.Color.Black
        Me.btnAdd.Image = CType(resources.GetObject("btnAdd.Image"), System.Drawing.Image)
        Me.btnAdd.Location = New System.Drawing.Point(602, 6)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(45, 30)
        Me.btnAdd.TabIndex = 2
        Me.btnAdd.Text = "+"
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Add Template")
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnDelete.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.Location = New System.Drawing.Point(703, 6)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(45, 30)
        Me.btnDelete.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Delete Template")
        Me.btnDelete.UseVisualStyleBackColor = True
        Me.btnDelete.Visible = False
        '
        'btnRemoveQuery
        '
        Me.btnRemoveQuery.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemoveQuery.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnRemoveQuery.ForeColor = System.Drawing.Color.Black
        Me.btnRemoveQuery.Image = Global.vLQMS.My.Resources.Resources.cancel
        Me.btnRemoveQuery.Location = New System.Drawing.Point(9, 413)
        Me.btnRemoveQuery.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRemoveQuery.Name = "btnRemoveQuery"
        Me.btnRemoveQuery.Size = New System.Drawing.Size(38, 28)
        Me.btnRemoveQuery.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.btnRemoveQuery, "Remove Row")
        Me.btnRemoveQuery.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.Location = New System.Drawing.Point(653, 6)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(45, 30)
        Me.btnEdit.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Edit Template")
        Me.btnEdit.UseVisualStyleBackColor = True
        Me.btnEdit.Visible = False
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
        Me.btnCancel.Location = New System.Drawing.Point(708, 422)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(92, 31)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnSave.ForeColor = System.Drawing.Color.Black
        Me.btnSave.Image = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.Location = New System.Drawing.Point(610, 422)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 31)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = True
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
        Me.btnClose.Location = New System.Drawing.Point(808, 422)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 31)
        Me.btnClose.TabIndex = 12
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'vsfColumn
        '
        Me.vsfColumn.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfColumn.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfColumn.AutoResize = True
        Me.vsfColumn.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfColumn.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfColumn.ExtendLastCol = True
        Me.vsfColumn.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.vsfColumn.Location = New System.Drawing.Point(668, 147)
        Me.vsfColumn.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfColumn.Name = "vsfColumn"
        Me.vsfColumn.Rows.DefaultSize = 21
        Me.vsfColumn.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfColumn.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfColumn.Size = New System.Drawing.Size(229, 262)
        Me.vsfColumn.StyleInfo = resources.GetString("vsfColumn.StyleInfo")
        Me.vsfColumn.TabIndex = 8
        '
        'PnlCopyFrom
        '
        Me.PnlCopyFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PnlCopyFrom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PnlCopyFrom.Controls.Add(Me.btnCopyFrom)
        Me.PnlCopyFrom.Controls.Add(Me.txtCopyItem)
        Me.PnlCopyFrom.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.PnlCopyFrom.Location = New System.Drawing.Point(513, 40)
        Me.PnlCopyFrom.Name = "PnlCopyFrom"
        Me.PnlCopyFrom.Size = New System.Drawing.Size(187, 33)
        Me.PnlCopyFrom.TabIndex = 295
        '
        'btnCopyFrom
        '
        Me.btnCopyFrom.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.btnCopyFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCopyFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnCopyFrom.ForeColor = System.Drawing.Color.Black
        Me.btnCopyFrom.Location = New System.Drawing.Point(10, 2)
        Me.btnCopyFrom.Name = "btnCopyFrom"
        Me.btnCopyFrom.Size = New System.Drawing.Size(123, 30)
        Me.btnCopyFrom.TabIndex = 0
        Me.btnCopyFrom.Text = "Copy From"
        Me.btnCopyFrom.UseVisualStyleBackColor = False
        '
        'txtCopyItem
        '
        Me.txtCopyItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCopyItem.BackColor = System.Drawing.Color.White
        Me.txtCopyItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCopyItem.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCopyItem.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtCopyItem.Location = New System.Drawing.Point(140, 2)
        Me.txtCopyItem.Margin = New System.Windows.Forms.Padding(4)
        Me.txtCopyItem.Name = "txtCopyItem"
        Me.txtCopyItem.Size = New System.Drawing.Size(37, 25)
        Me.txtCopyItem.TabIndex = 1
        Me.txtCopyItem.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(5, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(113, 20)
        Me.Label2.TabIndex = 297
        Me.Label2.Text = "Additionl Table"
        '
        'txtTable
        '
        Me.txtTable.BackColor = System.Drawing.SystemColors.Control
        Me.txtTable.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtTable.Location = New System.Drawing.Point(152, 45)
        Me.txtTable.MaxLength = 250
        Me.txtTable.Name = "txtTable"
        Me.txtTable.Size = New System.Drawing.Size(346, 26)
        Me.txtTable.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(5, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(141, 20)
        Me.Label3.TabIndex = 299
        Me.Label3.Text = "Additionl Condition"
        '
        'txtCondn
        '
        Me.txtCondn.BackColor = System.Drawing.SystemColors.Control
        Me.txtCondn.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtCondn.Location = New System.Drawing.Point(152, 79)
        Me.txtCondn.MaxLength = 250
        Me.txtCondn.Multiline = True
        Me.txtCondn.Name = "txtCondn"
        Me.txtCondn.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCondn.Size = New System.Drawing.Size(508, 59)
        Me.txtCondn.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(119, 413)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(379, 20)
        Me.Label4.TabIndex = 300
        Me.Label4.Text = "Click and drag values from grid to move up and down"
        Me.Label4.Visible = False
        '
        'frmUtlLabelDesign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(899, 465)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtCondn)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtTable)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.PnlCopyFrom)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnRemoveQuery)
        Me.Controls.Add(Me.vsfColumn)
        Me.Controls.Add(Me.btnload)
        Me.Controls.Add(Me.txtLblID)
        Me.Controls.Add(Me.CboTemplate)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.Label1)
        Me.ForeColor = System.Drawing.SystemColors.Control
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(915, 503)
        Me.Name = "frmUtlLabelDesign"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Label Design"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfColumn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlCopyFrom.ResumeLayout(False)
        Me.PnlCopyFrom.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents CboTemplate As ComboBox
    Friend WithEvents txtLblID As TextBox
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnload As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents vsfColumn As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnRemoveQuery As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents PnlCopyFrom As Panel
    Friend WithEvents btnCopyFrom As Button
    Friend WithEvents txtCopyItem As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtTable As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtCondn As TextBox
    Friend WithEvents Label4 As Label
End Class
