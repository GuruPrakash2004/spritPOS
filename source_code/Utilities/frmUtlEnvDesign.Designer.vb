<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmutlEnvDesign
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmutlEnvDesign))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtRptName = New System.Windows.Forms.TextBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblRptName = New System.Windows.Forms.Label()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.CboTemplate = New System.Windows.Forms.ComboBox()
        Me.txtEnvID = New System.Windows.Forms.TextBox()
        Me.btnload = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnRemoveQuery = New System.Windows.Forms.Button()
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
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(5, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Template"
        '
        'txtRptName
        '
        Me.txtRptName.BackColor = System.Drawing.Color.White
        Me.txtRptName.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtRptName.ForeColor = System.Drawing.Color.Black
        Me.txtRptName.Location = New System.Drawing.Point(131, 34)
        Me.txtRptName.MaxLength = 150
        Me.txtRptName.Name = "txtRptName"
        Me.txtRptName.Size = New System.Drawing.Size(346, 25)
        Me.txtRptName.TabIndex = 5
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
        Me.vsfDet.Size = New System.Drawing.Size(642, 270)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 8
        '
        'lblRptName
        '
        Me.lblRptName.AutoSize = True
        Me.lblRptName.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblRptName.ForeColor = System.Drawing.Color.Red
        Me.lblRptName.Location = New System.Drawing.Point(5, 38)
        Me.lblRptName.Name = "lblRptName"
        Me.lblRptName.Size = New System.Drawing.Size(48, 17)
        Me.lblRptName.TabIndex = 7
        Me.lblRptName.Tag = "M"
        Me.lblRptName.Text = "Report"
        '
        'CboTemplate
        '
        Me.CboTemplate.BackColor = System.Drawing.Color.White
        Me.CboTemplate.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.CboTemplate.ForeColor = System.Drawing.Color.Black
        Me.CboTemplate.FormattingEnabled = True
        Me.CboTemplate.Location = New System.Drawing.Point(131, 7)
        Me.CboTemplate.MaxLength = 150
        Me.CboTemplate.Name = "CboTemplate"
        Me.CboTemplate.Size = New System.Drawing.Size(346, 25)
        Me.CboTemplate.TabIndex = 0
        '
        'txtEnvID
        '
        Me.txtEnvID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtEnvID.ForeColor = System.Drawing.SystemColors.Control
        Me.txtEnvID.Location = New System.Drawing.Point(699, 7)
        Me.txtEnvID.Name = "txtEnvID"
        Me.txtEnvID.Size = New System.Drawing.Size(56, 25)
        Me.txtEnvID.TabIndex = 11
        Me.txtEnvID.Visible = False
        '
        'btnload
        '
        Me.btnload.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnload.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnload.ForeColor = System.Drawing.Color.Black
        Me.btnload.Location = New System.Drawing.Point(483, 7)
        Me.btnload.Name = "btnload"
        Me.btnload.Size = New System.Drawing.Size(86, 30)
        Me.btnload.TabIndex = 1
        Me.btnload.Text = "Load"
        Me.btnload.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnAdd.ForeColor = System.Drawing.Color.Black
        Me.btnAdd.Image = CType(resources.GetObject("btnAdd.Image"), System.Drawing.Image)
        Me.btnAdd.Location = New System.Drawing.Point(572, 7)
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
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnDelete.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.Location = New System.Drawing.Point(673, 7)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(45, 30)
        Me.btnDelete.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Delete Template")
        Me.btnDelete.UseVisualStyleBackColor = True
        Me.btnDelete.Visible = False
        '
        'btnEdit
        '
        Me.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.Location = New System.Drawing.Point(623, 7)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(45, 30)
        Me.btnEdit.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Edit")
        Me.btnEdit.UseVisualStyleBackColor = True
        Me.btnEdit.Visible = False
        '
        'btnRemoveQuery
        '
        Me.btnRemoveQuery.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveQuery.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnRemoveQuery.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemoveQuery.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnRemoveQuery.ForeColor = System.Drawing.Color.Black
        Me.btnRemoveQuery.Image = Global.vLQMS.My.Resources.Resources.cancel
        Me.btnRemoveQuery.Location = New System.Drawing.Point(8, 422)
        Me.btnRemoveQuery.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRemoveQuery.Name = "btnRemoveQuery"
        Me.btnRemoveQuery.Size = New System.Drawing.Size(38, 28)
        Me.btnRemoveQuery.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.btnRemoveQuery, "Remove Row")
        Me.btnRemoveQuery.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.Image = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.Location = New System.Drawing.Point(719, 426)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(86, 31)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel:Alt+C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnSave.ForeColor = System.Drawing.Color.Black
        Me.btnSave.Image = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.Location = New System.Drawing.Point(627, 426)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(86, 31)
        Me.btnSave.TabIndex = 11
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save:Alt+S")
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ForeColor = System.Drawing.Color.Black
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.Location = New System.Drawing.Point(811, 426)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(86, 31)
        Me.btnClose.TabIndex = 13
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close:Alt+O")
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
        Me.vsfColumn.Location = New System.Drawing.Point(657, 147)
        Me.vsfColumn.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfColumn.Name = "vsfColumn"
        Me.vsfColumn.Rows.DefaultSize = 21
        Me.vsfColumn.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfColumn.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfColumn.Size = New System.Drawing.Size(241, 270)
        Me.vsfColumn.StyleInfo = resources.GetString("vsfColumn.StyleInfo")
        Me.vsfColumn.TabIndex = 9
        '
        'PnlCopyFrom
        '
        Me.PnlCopyFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PnlCopyFrom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PnlCopyFrom.Controls.Add(Me.btnCopyFrom)
        Me.PnlCopyFrom.Controls.Add(Me.txtCopyItem)
        Me.PnlCopyFrom.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.PnlCopyFrom.Location = New System.Drawing.Point(483, 38)
        Me.PnlCopyFrom.Name = "PnlCopyFrom"
        Me.PnlCopyFrom.Size = New System.Drawing.Size(177, 33)
        Me.PnlCopyFrom.TabIndex = 295
        '
        'btnCopyFrom
        '
        Me.btnCopyFrom.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.btnCopyFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCopyFrom.ForeColor = System.Drawing.Color.Black
        Me.btnCopyFrom.Location = New System.Drawing.Point(0, 3)
        Me.btnCopyFrom.Name = "btnCopyFrom"
        Me.btnCopyFrom.Size = New System.Drawing.Size(123, 28)
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
        Me.txtCopyItem.Location = New System.Drawing.Point(130, 7)
        Me.txtCopyItem.Margin = New System.Windows.Forms.Padding(4)
        Me.txtCopyItem.Name = "txtCopyItem"
        Me.txtCopyItem.Size = New System.Drawing.Size(37, 25)
        Me.txtCopyItem.TabIndex = 1
        Me.txtCopyItem.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(5, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(96, 17)
        Me.Label2.TabIndex = 297
        Me.Label2.Text = "Additionl Table"
        '
        'txtTable
        '
        Me.txtTable.BackColor = System.Drawing.SystemColors.Control
        Me.txtTable.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtTable.Location = New System.Drawing.Point(131, 61)
        Me.txtTable.MaxLength = 250
        Me.txtTable.Name = "txtTable"
        Me.txtTable.Size = New System.Drawing.Size(346, 25)
        Me.txtTable.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(5, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 17)
        Me.Label3.TabIndex = 299
        Me.Label3.Text = "Additionl Condition"
        '
        'txtCondn
        '
        Me.txtCondn.BackColor = System.Drawing.SystemColors.Control
        Me.txtCondn.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtCondn.Location = New System.Drawing.Point(131, 88)
        Me.txtCondn.MaxLength = 250
        Me.txtCondn.Multiline = True
        Me.txtCondn.Name = "txtCondn"
        Me.txtCondn.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCondn.Size = New System.Drawing.Size(519, 50)
        Me.txtCondn.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(5, 450)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(295, 15)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Click and drag values from grid to move up and down"
        Me.Label4.Visible = False
        '
        'frmutlEnvDesign
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
        Me.Controls.Add(Me.txtEnvID)
        Me.Controls.Add(Me.CboTemplate)
        Me.Controls.Add(Me.lblRptName)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.txtRptName)
        Me.Controls.Add(Me.Label1)
        Me.ForeColor = System.Drawing.SystemColors.Control
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(915, 503)
        Me.Name = "frmutlEnvDesign"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Envelope Design"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfColumn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlCopyFrom.ResumeLayout(False)
        Me.PnlCopyFrom.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents txtRptName As TextBox
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblRptName As Label
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents CboTemplate As ComboBox
    Friend WithEvents txtEnvID As TextBox
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
